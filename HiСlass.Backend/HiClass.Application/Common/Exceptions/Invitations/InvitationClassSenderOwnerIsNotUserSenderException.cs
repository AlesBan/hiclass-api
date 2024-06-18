using HiClass.Application.Interfaces.Exceptions;
using static Newtonsoft.Json.JsonConvert;


namespace HiClass.Application.Common.Exceptions.Invitations;

public class InvitationClassSenderOwnerIsNotUserSenderException : Exception, IUiException
{
    private const string ExceptionMessageForUi = "ClassSender is not user sender's class.";

    public InvitationClassSenderOwnerIsNotUserSenderException(Guid userSenderId, Guid classSenderId) :
        base(CreateSerializedExceptionDto(userSenderId, classSenderId))
    {
    }

    private static string CreateSerializedExceptionDto(Guid userSenderId, Guid classSenderId)
    {
        var exceptionDto = new ExceptionResponseDto
        {
            ExceptionMessageForUi = ExceptionMessageForUi,
            ExceptionMessageForLogging = CreateMessageForLogging(userSenderId, classSenderId)
        };

        var serializedExceptionDto = SerializeObject(exceptionDto);

        return serializedExceptionDto;
    }

    private static string CreateMessageForLogging(Guid userSenderId, Guid classSenderId)
    {
        return $"{userSenderId} gets {nameof(InvitationClassSenderOwnerIsNotUserSenderException)} exception: " +
               $"User {userSenderId} attempted to send invitation, " +
               $"but classSenderId  ({classSenderId}) is not userSender's class.";
    }
}