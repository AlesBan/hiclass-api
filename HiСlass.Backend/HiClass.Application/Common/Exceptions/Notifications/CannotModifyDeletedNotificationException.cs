using HiClass.Application.Interfaces.Exceptions;
using HiClass.Domain.Enums;
using Newtonsoft.Json;
using System;

namespace HiClass.Application.Common.Exceptions.Notifications
{
    public class CannotModifyDeletedNotificationException : Exception, IUiException
    {
        private const string ExceptionMessageForUi = "Cannot modify a notification that is already Deleted.";

        public CannotModifyDeletedNotificationException(Guid notificationId) :
            base(CreateSerializedExceptionDto(notificationId))
        {
        }

        private static string CreateSerializedExceptionDto(Guid notificationId)
        {
            var exceptionDto = new ExceptionResponseDto
            {
                ExceptionMessageForUi = ExceptionMessageForUi,
                ExceptionMessageForLogging = CreateMessageForLogging(notificationId)
            };

            return JsonConvert.SerializeObject(exceptionDto);
        }

        private static string CreateMessageForLogging(Guid notificationId)
        {
            return $"{nameof(CannotModifyDeletedNotificationException)}: Cannot modify deleted notification {notificationId}.";
        }
    }
}