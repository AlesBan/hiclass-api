using HiClass.Domain.Enums;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.NotificationHandlers.Commands.UpdateNotificationStatus;

public class UpdateNotificationStatusCommand : IRequest
{
    public Guid NotificationId { get; set; }

    public NotificationStatus Status { get; set; }
}
