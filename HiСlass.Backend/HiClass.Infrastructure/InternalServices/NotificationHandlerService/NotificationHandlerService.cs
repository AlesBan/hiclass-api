using HiClass.Application.Handlers.EntityHandlers.NotificationHandlers.Commands;
using HiClass.Application.Handlers.EntityHandlers.NotificationHandlers.Queries;
using HiClass.Application.Models.Notifications;
using HiClass.Domain.Entities.Notifications;
using HiClass.Infrastructure.IntegrationServices.Firebase.FireBaseNotificationSender;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace HiClass.Infrastructure.InternalServices.NotificationHandlerService;

public class NotificationHandlerService : INotificationHandlerService
{
    private readonly IConfiguration _configuration;
    private readonly IFireBaseNotificationSender _fireBaseNotificationSender;

    public NotificationHandlerService(IFireBaseNotificationSender fireBaseNotificationSender, IConfiguration configuration)
    {
        _fireBaseNotificationSender = fireBaseNotificationSender;
        _configuration = configuration;
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
            NotificationType = notificationDto.NotificationType.ToString(),
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
}