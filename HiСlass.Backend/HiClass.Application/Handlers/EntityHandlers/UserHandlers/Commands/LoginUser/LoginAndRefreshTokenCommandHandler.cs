using AutoMapper;
using HiClass.Application.Common.Exceptions.User;
using HiClass.Application.Handlers.EntityConnectionHandlers.UserDeviceHandlers.Commands.CreateUserDevice;
using HiClass.Application.Helpers;
using HiClass.Application.Helpers.TokenHelper;
using HiClass.Application.Helpers.UserHelper;
using HiClass.Application.Interfaces;
using HiClass.Application.Models.User.Authentication;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.LoginUser;

public class LoginAndRefreshTokenCommandHandler : IRequestHandler<LoginAndRefreshTokenCommand, TokenModelResponseDto>
{
    private readonly ISharedLessonDbContext _context;
    private readonly IMapper _mapper;
    private readonly ITokenHelper _tokenHelper;
    private readonly IUserHelper _userHelper;
    private readonly IMediator _mediator;

    public LoginAndRefreshTokenCommandHandler(ISharedLessonDbContext context, IMapper mapper, ITokenHelper tokenHelper,
        IUserHelper userHelper, IMediator mediator)
    {
        _context = context;
        _mapper = mapper;
        _tokenHelper = tokenHelper;
        _userHelper = userHelper;
        _mediator = mediator;
    }

    public async Task<TokenModelResponseDto> Handle(LoginAndRefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(u => u.UserDevices)
            .ThenInclude(d => d.Device)
            .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

        if (user == null)
        {
            throw new UserNotFoundByEmailException(request.Email);
        }

        if (!user.IsPasswordSet)
        {
            throw new UserPasswordNotSetException(user.UserId);
        }
        
        PasswordHelper.VerifyPasswordHash(user, request.Password);

        var tokenUserDto = _mapper.Map<CreateTokenDto>(user);
        var newAccessToken = _tokenHelper.CreateAccessToken(tokenUserDto);
        var newRefreshToken = _tokenHelper.CreateRefreshToken(tokenUserDto);

        var userDevice = user.UserDevices
            .FirstOrDefault(ud => ud.Device?.DeviceToken == request.DeviceToken);

        if (userDevice != null)
        {
            userDevice.IsActive = true;
            userDevice.LastActive = DateTime.UtcNow;
            userDevice.RefreshToken = newRefreshToken;
            _context.UserDevices.Update(userDevice);
        }
        else
        {
            var command = new CreateUserDeviceCommand
            {
                DeviceToken = request.DeviceToken, 
                UserId = user.UserId,
                RefreshToken = newRefreshToken,
            };
            await _mediator.Send(command, cancellationToken);
        }

        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);

        return new TokenModelResponseDto()
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
        };
    }
}