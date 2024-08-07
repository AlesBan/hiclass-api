using HiClass.Application.Interfaces.Exceptions;
using static Newtonsoft.Json.JsonConvert;

namespace HiClass.Application.Common.Exceptions.Invitations;

public class InvitationReceiverIsSenderException : Exception, IUiException
{
    private const string ExceptionMessageForUi = "ClassReceiver can't be user's class.";

    public InvitationReceiverIsSenderException(Guid userSenderId, Guid classReceiverId) :
        base(CreateSerializedExceptionDto(userSenderId, classReceiverId))
    {
    }

    private static string CreateSerializedExceptionDto(Guid userSenderId, Guid classReceiverId)
    {
        var exceptionDto = new ExceptionResponseDto
        {
            ExceptionMessageForUi = ExceptionMessageForUi,
            ExceptionMessageForLogging = CreateMessageForLogging(userSenderId, classReceiverId)
        };

        var serializedExceptionDto = SerializeObject(exceptionDto);

        return serializedExceptionDto;
    }

    private static string CreateMessageForLogging(Guid userSenderId, Guid classReceiverId)
    {
        return $"{userSenderId} gets {nameof(InvitationReceiverIsSenderException)} exception: " +
               $"User {userSenderId} attempted to send invitation to class {classReceiverId} but is the owner of this class.";
    }
}