using HiClass.Application.Interfaces.Exceptions;
using static Newtonsoft.Json.JsonConvert;

namespace HiClass.Application.Common.Exceptions.Invitations;

public class InvalidUserPositionForClassReceiverInvitationException : Exception, IUiException
{
    private const string ExceptionMessageForUi =
        "Class receiver's owner does not have the required position for this invitation.";

    public InvalidUserPositionForClassReceiverInvitationException(Guid userSenderId, Guid userReceiverId,
        Guid classReceiverId) :
        base(CreateSerializedExceptionDto(userSenderId, userReceiverId, classReceiverId))
    {
    }

    private static string CreateSerializedExceptionDto(Guid userSenderId, Guid userReceiverId, Guid classReceiverId)
    {
        var exceptionDto = new ExceptionResponseDto
        {
            ExceptionMessageForUi = ExceptionMessageForUi,
            ExceptionMessageForLogging = CreateMessageForLogging(userSenderId, userReceiverId, classReceiverId)
        };

        var serializedExceptionDto = SerializeObject(exceptionDto);

        return serializedExceptionDto;
    }

    private static string CreateMessageForLogging(Guid userSenderId, Guid userReceiverId, Guid classReceiverId)
    {
        return $"{userSenderId} gets {nameof(InvalidUserPositionForClassReceiverInvitationException)} exception: " +
               $"User {userSenderId} attempted to send an invitation to class {classReceiverId}, but the class owner {userReceiverId} has the invalid position for this invitation.";
    }
}