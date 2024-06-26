using HiClass.Application.Common.Exceptions.Device;
using HiClass.Application.Common.Exceptions.Server.EmailManager;
using HiClass.Application.Common.Exceptions.User;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Notifications;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.DeviceHandlers.Commands.CreateDevice;

public class CreateDeviceCommandHandler : IRequestHandler<CreateDeviceCommand, Unit>
{
    private readonly ISharedLessonDbContext _context;

    public CreateDeviceCommandHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(CreateDeviceCommand request, CancellationToken cancellationToken)
    {
        if (request == null)
        {
            throw new MediatorCommandIsNullException();
        }

        var existingDevice =
            await _context.Devices.FirstOrDefaultAsync(x => x.DeviceToken == request.DeviceToken,
                cancellationToken);
        if (existingDevice != null && existingDevice.UserId == request.UserId) return Unit.Value;
        
        var newDevice = new Device
        {
            DeviceToken = request.DeviceToken,
            UserId = request.UserId
        };

        _context.Devices.Add(newDevice);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}