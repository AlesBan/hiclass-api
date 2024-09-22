using HiClass.Application.Interfaces.Exceptions;
using static Newtonsoft.Json.JsonConvert;

namespace HiClass.Application.Common.Exceptions.User;

public class UserPasswordAlreadySetException : Exception, IUiException
{
    private const string ExceptionMessageForUi = "Password is already set for this user.";

    public UserPasswordAlreadySetException(Guid userId) : base(CreateSerializedExceptionDto(userId))
    {
    }

    private static string CreateSerializedExceptionDto(Guid userId)
    {
        var exceptionDto = new ExceptionResponseDto
        {
            ExceptionMessageForUi = ExceptionMessageForUi,
            ExceptionMessageForLogging = CreateMessageForLogging(userId)
        };

        var serializedExceptionDto = SerializeObject(exceptionDto);

        return serializedExceptionDto;
    }

    private static string CreateSerializedExceptionDto(Guid userId, string message)
    {
        var exceptionDto = new ExceptionResponseDto
        {
            ExceptionMessageForUi = message,
            ExceptionMessageForLogging = CreateMessageForLogging(userId)
        };

        var serializedExceptionDto = SerializeObject(exceptionDto);

        return serializedExceptionDto;
    }

    private static string CreateMessageForLogging(Guid userId)
    {
        return $"Password for user with {userId} id is already set.";
    }
}
