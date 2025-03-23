using HiClass.Application.Common.Exceptions.Device;
using HiClass.Application.Common.Exceptions.User;
using HiClass.Application.Interfaces;
using HiClass.Domain.EntityConnections;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.RevokeRefreshToken;

public class RevokeRefreshTokenCommandHandler : IRequestHandler<RevokeRefreshTokenCommand, Unit>
{
    private readonly ISharedLessonDbContext _context;

    public RevokeRefreshTokenCommandHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(u => u.UserDevices)
            .ThenInclude(d => d.Device)
            .FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);

        if (user is null)
        {
            throw new UserNotFoundByIdException(request.UserId);
        }

        UserDevice userDevice;

        if (!string.IsNullOrEmpty(request.DeviceToken))
        {
            userDevice = user.UserDevices
                .FirstOrDefault(ud => ud.Device?.DeviceToken == request.DeviceToken)!;

            if (userDevice is null)
            {
                throw new UserDeviceNotFoundByDeviceTokenException(request.DeviceToken);
            }
        }
        // Если DeviceToken не передан, ищем устройство по RefreshToken
        else if (!string.IsNullOrEmpty(request.RefreshToken))
        {
            userDevice = user.UserDevices
                .FirstOrDefault(ud => ud.RefreshToken == request.RefreshToken)!;

            if (userDevice is null)
            {
                throw new UserDeviceNotFoundByRefreshTokenException(request.RefreshToken);
            }
        }
        else
        {
            throw new ArgumentException("Either DeviceToken or RefreshToken must be provided.");
        }

        userDevice.RefreshToken = null;
        userDevice.IsActive = false;
        userDevice.LastActive = DateTime.UtcNow;

        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}