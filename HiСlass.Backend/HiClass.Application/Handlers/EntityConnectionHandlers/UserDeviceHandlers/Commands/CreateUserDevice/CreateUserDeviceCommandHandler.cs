using HiClass.Application.Common.Exceptions.Server.EmailManager;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Notifications;
using HiClass.Domain.EntityConnections;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityConnectionHandlers.UserDeviceHandlers.Commands.CreateUserDevice;

public class CreateUserDeviceCommandHandler : IRequestHandler<CreateUserDeviceCommand, Unit>
{
    private readonly ISharedLessonDbContext _context;

    public CreateUserDeviceCommandHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(CreateUserDeviceCommand request, CancellationToken cancellationToken)
    {
        if (request == null)
        {
            throw new MediatorCommandIsNullException();
        }

        var existingDevice = await _context.Devices
            .Include(x => x.UserDevices)
            .FirstOrDefaultAsync(x => x.DeviceToken == request.DeviceToken, cancellationToken);

        if (existingDevice == null)
        {
            existingDevice = new Device
            {
                DeviceId = Guid.NewGuid(),
                DeviceToken = request.DeviceToken,
            };
            _context.Devices.Add(existingDevice);
            await _context.SaveChangesAsync(cancellationToken);
        }

        var existingUserDevice = existingDevice.UserDevices
            .FirstOrDefault(ud => ud.UserId == request.UserId);

        if (existingUserDevice != null)
        {
            return Unit.Value; 
        }

        var userDevice = new UserDevice
        {
            UserId = request.UserId,
            DeviceId = existingDevice.DeviceId,
            RefreshToken = request.RefreshToken,
            IsActive = true
        };

        _context.UserDevices.Add(userDevice);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

}