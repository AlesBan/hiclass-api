using HiClass.Application.Interfaces.Exceptions;
using HiClass.Domain.Enums;
using Newtonsoft.Json;

namespace HiClass.Application.Common.Exceptions.Notifications
{
    public class SameStatusUpdateException : Exception, IUiException
    {
        private const string ExceptionMessageForUi = "The notification already has the specified status.";

        public SameStatusUpdateException(Guid notificationId, NotificationStatus status) :
            base(CreateSerializedExceptionDto(notificationId, status))
        {
        }

        private static string CreateSerializedExceptionDto(Guid notificationId, NotificationStatus status)
        {
            var exceptionDto = new ExceptionResponseDto
            {
                ExceptionMessageForUi = ExceptionMessageForUi,
                ExceptionMessageForLogging = CreateMessageForLogging(notificationId, status)
            };

            var serializedExceptionDto = JsonConvert.SerializeObject(exceptionDto);

            return serializedExceptionDto;
        }

        private static string CreateMessageForLogging(Guid notificationId, NotificationStatus status)
        {
            return $"{nameof(SameStatusUpdateException)}: Notification {notificationId} is already in status {status}.";
        }
    }
}