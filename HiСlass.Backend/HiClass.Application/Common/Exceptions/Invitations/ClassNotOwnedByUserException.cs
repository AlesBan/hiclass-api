using HiClass.Application.Interfaces.Exceptions;
using static Newtonsoft.Json.JsonConvert;

namespace HiClass.Application.Common.Exceptions.Invitations;

public class ClassNotOwnedByUserException : Exception, IUiForbiddenException
{
    private const string ExceptionMessageForUi = "The class does not belong to the user.";

    public ClassNotOwnedByUserException(Guid userSenderId, Guid classSenderId) :
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
        return $"{userSenderId} gets {nameof(ClassNotOwnedByUserException)} exception: " +
               $"User {userSenderId} attempted to send an invitation, " +
               $"but class ({classSenderId}) is not owned by the user.";
    }
}