using HiClass.Domain.Entities.Notifications;
using HiClass.Domain.Enums;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.NotificationHandlers.Commands.CreateNotification;

public class CreateNotificationCommand : IRequest<Unit>
{
    public NotificationType NotificationType { get; set; } 
    public string Message { get; set; } = string.Empty;
    public Guid UserReceiverId { get; set; }
}