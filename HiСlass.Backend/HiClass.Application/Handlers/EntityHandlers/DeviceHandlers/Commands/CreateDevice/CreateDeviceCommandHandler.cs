using HiClass.Application.Common.Exceptions.Device;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Notifications;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.DeviceHandlers.Commands.CreateDevice;

public class CreateDeviceCommandHandler : IRequestHandler<CreateDeviceCommand, Device>
{
    private readonly ISharedLessonDbContext _context;

    public CreateDeviceCommandHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<Device> Handle(CreateDeviceCommand request, CancellationToken cancellationToken)
    {
        var deviceToken = request.DeviceToken;

        var device =
            await _context.Devices.FindAsync(new object[] { deviceToken }, cancellationToken: cancellationToken);

        if (device != null)
        {
            throw new DeviceAlreadyExistsException();
        }

        device = new Device
        {
            DeviceToken = deviceToken
        };

        _context.Devices.Add(device);

        await _context.SaveChangesAsync(cancellationToken);

        return device;
    }
}