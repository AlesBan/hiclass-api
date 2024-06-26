using HiClass.Application.Common.Exceptions.Device;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Notifications;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.DeviceHandlers.Queries.GetDeviceByToken;

public class GetDeviceByTokenQueryHandler : IRequestHandler<GetDeviceByTokenQuery, Device>
{
    private readonly ISharedLessonDbContext _context;

    public GetDeviceByTokenQueryHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<Device> Handle(GetDeviceByTokenQuery request, CancellationToken cancellationToken)
    {
        var deviceToken = request.DeviceToken;
        var device = await _context.Devices.FirstOrDefaultAsync(x => x.DeviceToken == deviceToken,
            cancellationToken: cancellationToken);

        if (device == null)
        {
            throw new DeviceByTokenNotFoundException(deviceToken);
        }

        return device;
    }
}