using AutoMapper;
using HiClass.Application.Common.Exceptions.User;
using HiClass.Application.Handlers.EntityConnectionHandlers.UserDeviceHandlers.Commands.CreateUserDevice;
using HiClass.Application.Helpers;
using HiClass.Application.Helpers.TokenHelper;
using HiClass.Application.Interfaces;
using HiClass.Application.Models.User.Authentication;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserPassword;

public class UpdateUserPasswordCommandHandler : IRequestHandler<UpdateUserPasswordCommand, TokenModelResponseDto>
{
    private readonly ISharedLessonDbContext _context;
    private readonly IMapper _mapper;
    private readonly ITokenHelper _tokenHelper;
    private readonly IMediator _mediator;

    public UpdateUserPasswordCommandHandler(ISharedLessonDbContext context, ITokenHelper tokenHelper, IMapper mapper,
        IMediator mediator)
    {
        _context = context;
        _tokenHelper = tokenHelper;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<TokenModelResponseDto> Handle(UpdateUserPasswordCommand request,
        CancellationToken cancellationToken)
    {
        var user = _context.Users
            .FirstOrDefault(u =>
                u.UserId == request.UserId);

        if (user == null)
        {
            throw new UserNotFoundByIdException(request.UserId);
        }

        PasswordHelper.CreatePasswordHash(request.NewPassword, out var passwordHash, out var passwordSalt);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);

        var tokenUserDto = _mapper.Map<CreateTokenDto>(user);
        var newAccessToken = _tokenHelper.CreateAccessToken(tokenUserDto);
        var newRefreshToken = _tokenHelper.CreateRefreshToken(tokenUserDto);

        var userDevice = user.UserDevices
            .FirstOrDefault(ud => ud.Device.DeviceToken == request.DeviceToken);

        if (userDevice != null)
        {
            userDevice.IsActive = true;
            userDevice.LastActive = DateTime.UtcNow;
            userDevice.RefreshToken = newRefreshToken;
            _context.UserDevices.Update(userDevice);
            await _context.SaveChangesAsync(CancellationToken.None);
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

        return new TokenModelResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }
}