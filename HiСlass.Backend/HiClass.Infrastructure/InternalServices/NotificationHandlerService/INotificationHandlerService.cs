using HiClass.Application.Models.Notifications;
using HiClass.Domain.Entities.Notifications;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace HiClass.Infrastructure.InternalServices.NotificationHandlerService;

public interface INotificationHandlerService
{
    Task<List<Notification>> GetUserNotificationsByUserId(Guid userId, IMediator mediator);
    Task ProcessNotification(NotificationDto notificationDto, IMediator mediator);
    Task UpdateNotificationStatus(UpdateNotificationStatusRequestDto updateNotificationStatus, IMediator mediator);
}