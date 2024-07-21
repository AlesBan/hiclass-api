using HiClass.Application.Handlers.EntityHandlers.DeviceHandlers.Queries.GetAllUserDevicesByUserId;
using HiClass.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityConnectionHandlers.UserDeviceHandlers.Queries.GetAllUserDevicesByUserId;

public class GetActiveUserDevicesByUserIdCommandHandler :
    IRequestHandler<GetActiveUserDevicesByUserIdCommand, List<string>>
{
    private readonly ISharedLessonDbContext _context;

    public GetActiveUserDevicesByUserIdCommandHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<List<string>> Handle(GetActiveUserDevicesByUserIdCommand request, CancellationToken cancellationToken)
    {
        var devices = await _context.UserDevices
            .Include(ud => ud.Device)
            .Where(ud => ud.UserId == request.UserId && ud.IsActive && ud.Device != null)
            .Select(ud => ud.Device!.DeviceToken)
            .ToListAsync(cancellationToken);

        return devices;
    }

}