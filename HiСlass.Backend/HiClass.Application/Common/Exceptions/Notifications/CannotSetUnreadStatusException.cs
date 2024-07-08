using HiClass.Application.Interfaces.Exceptions;
using HiClass.Domain.Enums;
using Newtonsoft.Json;
using System;

namespace HiClass.Application.Common.Exceptions.Notifications
{
    public class CannotSetUnreadStatusException : Exception, IUiException
    {
        private const string ExceptionMessageForUi = "Cannot set notification status to Unread when it is already Read.";

        public CannotSetUnreadStatusException(Guid notificationId, NotificationStatus currentStatus) :
            base(CreateSerializedExceptionDto(notificationId, currentStatus))
        {
        }

        private static string CreateSerializedExceptionDto(Guid notificationId, NotificationStatus currentStatus)
        {
            var exceptionDto = new ExceptionResponseDto
            {
                ExceptionMessageForUi = ExceptionMessageForUi,
                ExceptionMessageForLogging = CreateMessageForLogging(notificationId, currentStatus)
            };

            return JsonConvert.SerializeObject(exceptionDto);
        }

        private static string CreateMessageForLogging(Guid notificationId, NotificationStatus currentStatus)
        {
            return $"{nameof(CannotSetUnreadStatusException)}: Cannot set notification {notificationId} status to Unread because it is already in status {currentStatus}.";
        }
    }
}