using HiClass.Application.Interfaces.Exceptions;
using static Newtonsoft.Json.JsonConvert;


namespace HiClass.Application.Common.Exceptions.User;

public class UserNotFoundByIdException : Exception, IUiException
{
    private const string ExceptionMessageForUi = "User not found.";

    public UserNotFoundByIdException(Guid userId) : base(CreateSerializedExceptionDto(userId))
    {
    }

    public UserNotFoundByIdException(Guid userId, string message) :
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
        return $"User with {userId} id was not found.";
    }
}