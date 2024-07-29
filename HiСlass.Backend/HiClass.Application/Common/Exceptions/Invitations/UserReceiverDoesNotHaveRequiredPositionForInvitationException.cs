using HiClass.Application.Interfaces.Exceptions;
using static Newtonsoft.Json.JsonConvert;

namespace HiClass.Application.Common.Exceptions.Invitations
{
    public class UserReceiverDoesNotHaveRequiredPositionForInvitationException : Exception, IUiException
    {
        private const string ExceptionMessageForUi = "User receiver of the invitation does not have the required position.";

        public UserReceiverDoesNotHaveRequiredPositionForInvitationException(Guid userSenderId, Guid userReceiverId) :
            base(CreateSerializedExceptionDto(userSenderId, userReceiverId))
        {
        }

        private static string CreateSerializedExceptionDto(Guid userSenderId, Guid userReceiverId)
        {
            var exceptionDto = new ExceptionResponseDto
            {
                ExceptionMessageForUi = ExceptionMessageForUi,
                ExceptionMessageForLogging = CreateMessageForLogging(userSenderId, userReceiverId)
            };

            var serializedExceptionDto = SerializeObject(exceptionDto);

            return serializedExceptionDto;
        }

        private static string CreateMessageForLogging(Guid userSenderId, Guid userReceiverId)
        {
            return $"{userSenderId} gets {nameof(UserReceiverDoesNotHaveRequiredPositionForInvitationException)} exception: " +
                   $"User {userSenderId} attempted to send an invitation to user {userReceiverId}, but the user receiver does not have the required position for this invitation.";
        }
    }
}