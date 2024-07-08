using HiClass.Application.Handlers.EntityHandlers.NotificationHandlers.Commands;
using HiClass.Application.Handlers.EntityHandlers.NotificationHandlers.Commands.UpdateNotificationStatus;
using HiClass.Application.Handlers.EntityHandlers.NotificationHandlers.Queries;
using HiClass.Application.Models.Notifications;
using HiClass.Domain.Entities.Notifications;
using HiClass.Infrastructure.IntegrationServices.Firebase.FireBaseNotificationSender;
using MediatR;

namespace HiClass.Infrastructure.InternalServices.NotificationHandlerService;

public class NotificationHandlerService : INotificationHandlerService
{
    private readonly IFireBaseNotificationSender _fireBaseNotificationSender;

    public NotificationHandlerService(IFireBaseNotificationSender fireBaseNotificationSender)
    {
        _fireBaseNotificationSender = fireBaseNotificationSender;
    }

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

        var notification = await mediator.Send(command);

        return notification;
    }

    public Task SendNotificationAsync(NotificationResponseDto notificationDto, List<string> deviceTokens)
    {
        _fireBaseNotificationSender.SendNotificationAsync(notificationDto, deviceTokens);
        return Task.CompletedTask;
    }

    public Task UpdateNotificationStatus(UpdateNotificationStatusRequestDto updateNotificationStatus, IMediator mediator)
    {
        var command = new UpdateNotificationStatusCommand
        {
            NotificationId = updateNotificationStatus.NotificationId,
            Status = updateNotificationStatus.Status
        };
        
        return mediator.Send(command);
    }
}