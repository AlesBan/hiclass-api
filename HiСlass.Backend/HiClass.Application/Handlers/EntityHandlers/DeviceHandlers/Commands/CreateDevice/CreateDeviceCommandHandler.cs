using HiClass.Application.Common.Exceptions.Device;
using HiClass.Application.Common.Exceptions.Server.EmailManager;
using HiClass.Application.Common.Exceptions.User;
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
        if (request == null)
        {
            throw new MediatorCommandIsNullException();
        }

        var existingDevice = await _context.Devices.FindAsync(new object[] { request.DeviceToken }, cancellationToken);
        if (existingDevice != null)
        {
            throw new DeviceAlreadyExistsException();
        }

        var user = await _context.Users.FindAsync(new object[] { request.UserId }, cancellationToken);
        if (user == null)
        {
            throw new UserNotFoundByIdException(userId: request.UserId);
        }

        var newDevice = new Device
        {
            DeviceToken = request.DeviceToken,
            UserId = request.UserId
        };

        _context.Devices.Add(newDevice);
        await _context.SaveChangesAsync(cancellationToken);

        return newDevice;
    }
}