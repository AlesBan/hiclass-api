using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Notifications;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.NotificationHandlers.Commands.CreateNotification;

public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, Unit>
{
    private readonly ISharedLessonDbContext _context;

    public CreateNotificationCommandHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = new Notification
        {
            UserReceiverId = request.UserReceiverId,
            Type = request.NotificationType,
            Message = request.Message
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}