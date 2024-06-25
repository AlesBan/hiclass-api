using HiClass.Domain.Entities.Notifications;
using HiClass.Domain.Enums;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.NotificationHandlers.Commands;

public class CreateNotificationCommand : IRequest<Notification>
{
    public string NotificationType { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public Guid UserReceiverId { get; set; }
}