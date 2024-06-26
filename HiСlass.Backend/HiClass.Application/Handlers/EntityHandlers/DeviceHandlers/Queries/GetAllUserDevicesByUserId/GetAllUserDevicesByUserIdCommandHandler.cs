using HiClass.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.DeviceHandlers.Queries.GetAllUserDevicesByUserId;

public class GetAllUserDevicesByUserIdCommandHandler :
    IRequestHandler<GetAllUserDevicesByUserIdCommand, List<string>>
{
    private readonly ISharedLessonDbContext _context;

    public GetAllUserDevicesByUserIdCommandHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<List<string>> Handle(GetAllUserDevicesByUserIdCommand request,
        CancellationToken cancellationToken)
    {
        var devices = await _context.Devices
            .Where(x => x.UserId == request.UserId)
            .Select(x => x.DeviceToken)
            .ToListAsync(cancellationToken: cancellationToken);
        return devices;
    }
}