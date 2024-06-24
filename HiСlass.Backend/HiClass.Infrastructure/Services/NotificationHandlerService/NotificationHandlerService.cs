using HiClass.Application.Handlers.EntityHandlers.NotificationHandlers.Commands;
using HiClass.Application.Handlers.EntityHandlers.NotificationHandlers.Queries;
using HiClass.Application.Models.Notifications;
using HiClass.Domain.Entities.Notifications;
using MediatR;

namespace HiClass.Infrastructure.Services.NotificationHandlerService;

public class NotificationHandlerService : INotificationHandlerService
{
    public async Task<List<Notification>> GetUserNotificationsByUserId(Guid userId, IMediator mediator)
    {
        var command = new GetUserNotificationsByUserIdQuery
        {
            UserId = userId
        };

        var notifications = await mediator.Send(command);
        return notifications;
    }

    public async Task<Notification> CreateNotification(NotificationDto notificationDto, IMediator mediator)
    {
        var command = new CreateNotificationCommand
        {
            UserReceiverId = notificationDto.UserReceiverId,
            NotificationType = notificationDto.NotificationType,
            Message = notificationDto.NotificationMessage.Message
        };

        await mediator.Send(command);
    }
}