using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Notifications;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.NotificationHandlers.Commands;

public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, Notification>
{
    private readonly ISharedLessonDbContext _context;

    public CreateNotificationCommandHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<Notification> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = new Notification
        {
            UserReceiverId = request.UserReceiverId,
            Type = request.NotificationType,
            Message = request.Message
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync(cancellationToken);

        return notification;
    }
}