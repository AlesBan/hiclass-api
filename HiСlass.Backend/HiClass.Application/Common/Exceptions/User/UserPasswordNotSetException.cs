using HiClass.Application.Interfaces.Exceptions;
using static Newtonsoft.Json.JsonConvert;

namespace HiClass.Application.Common.Exceptions.User;

public class UserPasswordNotSetException : Exception, IUiException
{
    private const string ExceptionMessageForUi = "Password is not set for this user.";

    public UserPasswordNotSetException(Guid userId) : base(CreateSerializedExceptionDto(userId))
    {
    }

    public UserPasswordNotSetException(Guid userId, string message) :
        base(CreateSerializedExceptionDto(userId, message))
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
        return $"Password for user with {userId} id is not set.";
    }
}