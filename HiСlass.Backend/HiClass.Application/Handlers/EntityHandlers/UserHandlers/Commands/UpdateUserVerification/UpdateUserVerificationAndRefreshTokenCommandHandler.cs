using AutoMapper;
using HiClass.Application.Common.Exceptions.User;
using HiClass.Application.Handlers.EntityConnectionHandlers.UserDeviceHandlers.Commands.CreateUserDevice;
using HiClass.Application.Helpers.TokenHelper;
using HiClass.Application.Interfaces;
using HiClass.Application.Models.User.Authentication;
using HiClass.Domain.Entities.Main;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserVerification;

public class
    UpdateUserVerificationAndRefreshTokenCommandHandler : IRequestHandler<UpdateUserVerificationAndRefreshTokenCommand,
    TokenModelResponseDto>
{
    private readonly ISharedLessonDbContext _context;
    private readonly ITokenHelper _tokenHelper;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public UpdateUserVerificationAndRefreshTokenCommandHandler(ISharedLessonDbContext sharedLessonDbContext,
        ITokenHelper tokenHelper,
        IMapper mapper, IMediator mediator)
    {
        _context = sharedLessonDbContext;
        _tokenHelper = tokenHelper;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<TokenModelResponseDto> Handle(UpdateUserVerificationAndRefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        User? user;

        if (request.UserId != Guid.Empty)
        {
            user = await _context.Users
                .Include(u => u.UserDevices)
                .ThenInclude(d => d.Device)
                .FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);
        }
        else
        {
            user = await _context.Users
                .Include(u => u.UserDevices)
                .ThenInclude(d => d.Device)
                .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);
        }

        if (user == null)
        {
            throw new UserNotFoundByIdException(request.UserId);
        }

        if (user.VerificationCode != request.VerificationCode)
        {
            throw new InvalidVerificationCodeProvidedException(user.UserId, request.VerificationCode);
        }

        user.IsVerified = true;
        user.VerifiedAt = DateTime.UtcNow;

        _context.Users.Update(user);
        await _context.SaveChangesAsync(CancellationToken.None);

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

        return new TokenModelResponseDto()
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }
}