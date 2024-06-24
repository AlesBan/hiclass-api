using HiClass.Domain.Entities.Notifications;
using HiClass.Domain.Enums;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.NotificationHandlers.Commands;

public class CreateNotificationCommand : IRequest<Notification>
{
    public NotificationType NotificationType { get; set; } 
    public string Message { get; set; } = string.Empty;
    public Guid UserReceiverId { get; set; }
}