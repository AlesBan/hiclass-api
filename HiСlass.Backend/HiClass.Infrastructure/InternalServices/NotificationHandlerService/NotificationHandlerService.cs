using HiClass.Application.Handlers.EntityHandlers.NotificationHandlers.Commands.CreateNotification;
using HiClass.Application.Handlers.EntityHandlers.NotificationHandlers.Commands.UpdateNotificationStatus;
using HiClass.Application.Handlers.EntityHandlers.NotificationHandlers.Queries;
using HiClass.Application.Models.Notifications;
using HiClass.Domain.Entities.Notifications;
using HiClass.Infrastructure.IntegrationServices.Firebase.FireBaseNotificationSender;
using HiClass.Infrastructure.InternalServices.DeviceHandlerService;
using MediatR;

namespace HiClass.Infrastructure.InternalServices.NotificationHandlerService;

public class NotificationHandlerService : INotificationHandlerService
{
    private readonly IFireBaseNotificationSender _fireBaseNotificationSender;
    private readonly IDeviceHandlerService _deviceHandlerService;

    public NotificationHandlerService(IFireBaseNotificationSender fireBaseNotificationSender, IDeviceHandlerService deviceHandlerService)
    {
        _fireBaseNotificationSender = fireBaseNotificationSender;
        _deviceHandlerService = deviceHandlerService;
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

    public async Task ProcessNotification(NotificationDto notificationDto, IMediator mediator)
    {
        await CreateNotification(notificationDto, mediator);
        var userDeviceTokens = await GetUserDeviceTokens(notificationDto.UserReceiverId, mediator);
        await SendNotificationAsync(notificationDto, userDeviceTokens);
    }
    private static async Task CreateNotification(NotificationDto notificationDto, IMediator mediator)
    {
        var command = new CreateNotificationCommand
        {
            UserReceiverId = notificationDto.UserReceiverId,
            NotificationType = notificationDto.NotificationType,
            Message = notificationDto.NotificationMessage.Message
        };

        await mediator.Send(command);
    }

    private async Task<List<string>> GetUserDeviceTokens(Guid userId, IMediator mediator)
    {
        return await _deviceHandlerService.GetActiveUserDeviceTokensByUserId(userId, mediator);
    }

    private Task SendNotificationAsync(NotificationDto notificationDto, List<string> deviceTokens)
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