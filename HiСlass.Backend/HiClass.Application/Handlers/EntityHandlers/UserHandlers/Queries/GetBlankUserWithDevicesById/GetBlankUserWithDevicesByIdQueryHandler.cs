using HiClass.Application.Common.Exceptions.User;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Main;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetBlankUserWithDevicesById;

public class GetBlankUserWithDevicesByIdQueryHandler : IRequestHandler<GetBlankUserWithDevicesByIdQuery, User>
{
    private readonly ISharedLessonDbContext _context;

    public GetBlankUserWithDevicesByIdQueryHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<User> Handle(GetBlankUserWithDevicesByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Include(u => u.UserDevices)
            .ThenInclude(ud => ud.Device)
            .FirstOrDefaultAsync(u => u.UserId == request.UserId, cancellationToken: cancellationToken);

        if (user == null)
        {
            throw new UserNotFoundByIdException(request.UserId);
        }

        return user;
    }
}