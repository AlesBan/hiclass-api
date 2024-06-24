using HiClass.Application.Models.Notifications;
using HiClass.Domain.Entities.Notifications;
using MediatR;

namespace HiClass.Infrastructure.Services.NotificationHandlerService;

public interface INotificationHandlerService
{
    Task<List<Notification>> GetUserNotificationsByUserId(Guid userId, IMediator mediator);
    Task<Notification> CreateNotification(NotificationDto notificationDto, IMediator mediator);
}