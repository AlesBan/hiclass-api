using AutoMapper;
using HiClass.Application.Common.Exceptions.User;
using HiClass.Application.Handlers.EntityConnectionHandlers.UserDeviceHandlers.Commands.CreateUserDevice;
using HiClass.Application.Helpers;
using HiClass.Application.Helpers.TokenHelper;
using HiClass.Application.Interfaces;
using HiClass.Application.Models.User.Authentication;
using HiClass.Domain.EntityConnections;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        var user = await _context.Users
            .Include(u => u.UserDevices)
            .ThenInclude(d => d.Device)
            .FirstOrDefaultAsync(u => u.UserId == request.UserId, cancellationToken);

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

        UserDevice userDevice;

        if (!string.IsNullOrEmpty(request.DeviceToken))
        {
            userDevice = user.UserDevices
                .FirstOrDefault(ud => ud.Device?.DeviceToken == request.DeviceToken)!;
        }
        else if (!string.IsNullOrEmpty(request.RefreshToken))
        {
            userDevice = user.UserDevices
                .FirstOrDefault(ud => ud.RefreshToken == request.RefreshToken)!;
        }
        else
        {
            userDevice = null;
        }

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
                DeviceToken = request.DeviceToken, // Может быть null
                UserId = user.UserId,
                RefreshToken = newRefreshToken,
            };
            await _mediator.Send(command, cancellationToken);
        }

        await _context.SaveChangesAsync(CancellationToken.None);

        return new TokenModelResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }
}