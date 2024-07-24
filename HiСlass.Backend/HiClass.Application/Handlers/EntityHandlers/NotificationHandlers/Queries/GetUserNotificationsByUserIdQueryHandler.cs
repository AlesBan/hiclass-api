using HiClass.Application.Common.Exceptions.User;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Notifications;
using HiClass.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.NotificationHandlers.Queries;

public class GetUserNotificationsByUserIdQueryHandler :
    IRequestHandler<GetUserNotificationsByUserIdQuery, List<Notification>>
{
    private readonly ISharedLessonDbContext _context;

    public GetUserNotificationsByUserIdQueryHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<List<Notification>> Handle(GetUserNotificationsByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var userId = request.UserId;
        _ = await _context.Users.FindAsync(new object?[] { userId }, cancellationToken: cancellationToken) ??
            throw new UserNotFoundByIdException(userId);

        var notifications = await _context.Notifications
            .Where(x => x.UserReceiverId == userId && x.Status == NotificationStatus.Unread)
            .ToListAsync(cancellationToken);
        return notifications;
    }
}