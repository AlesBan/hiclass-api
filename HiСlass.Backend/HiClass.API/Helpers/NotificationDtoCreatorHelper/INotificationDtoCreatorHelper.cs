using HiClass.Application.Models.Notifications;
using HiClass.Domain.Enums;

namespace HiClass.API.Helpers.NotificationDtoCreatorHelper;

public interface INotificationDtoCreatorHelper
{
    NotificationDto CreateNotificationDto(Guid userReceiverId, NotificationType notificationType, string sendersEmail);
}