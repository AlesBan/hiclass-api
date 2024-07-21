using HiClass.Application.Models.Notifications;
using HiClass.Domain.Entities.Notifications;
using HiClass.Domain.Enums;

namespace HiClass.API.Helpers.NotificationDtoCreatorHelper;

public class NotificationDtoCreatorHelper : INotificationDtoCreatorHelper
{
    public NotificationDto CreateNotificationDto(Guid userReceiverId, NotificationType notificationType, string
        sendersEmail)
    {
        return new NotificationDto
        {
            UserReceiverId = userReceiverId,
            NotificationType = notificationType,
            NotificationMessage = new NotificationMessage
            {
                Sender = sendersEmail,
                Message = $"{sendersEmail} sent you an invitation to join his/her class"
            }
        };
    }
}