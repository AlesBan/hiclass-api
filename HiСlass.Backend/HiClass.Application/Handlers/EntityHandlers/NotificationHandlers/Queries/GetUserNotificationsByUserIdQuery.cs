using HiClass.Domain.Entities.Notifications;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.NotificationHandlers.Queries;

public class GetUserNotificationsByUserIdQuery : IRequest<List<Notification>>
{
    public Guid UserId { get; set; }
}