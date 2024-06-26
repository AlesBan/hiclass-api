using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Notifications;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.DeviceHandlers.Commands.DeleteDeviceByToken;

public class DeleteDeviceByTokenCommandHandler : IRequestHandler<DeleteDeviceByTokenCommand, Unit>
{
    private readonly ISharedLessonDbContext _context;

    public DeleteDeviceByTokenCommandHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteDeviceByTokenCommand request, CancellationToken cancellationToken)
    {
        var device = await _context.Devices.FirstOrDefaultAsync(x => x.DeviceToken == request.DeviceToken,
            cancellationToken: cancellationToken);

        if (device == null)
        {
            throw new NotFoundException(nameof(Device), request.DeviceToken);
        }

        _context.Devices.Remove(device);
        await _context.SaveChangesAsync(CancellationToken.None);

        return Unit.Value;
    }
}