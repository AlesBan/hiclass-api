using HiClass.Application.Interfaces.Exceptions;
using static Newtonsoft.Json.JsonConvert;


namespace HiClass.Application.Common.Exceptions.User;

public class InvalidInputCredentialsException : Exception, IUiException
{
    public InvalidInputCredentialsException(string message) : base(CreateSerializedExceptionDto(message))
    {
    }

    private static string CreateSerializedExceptionDto(string message)
    {
        var exceptionDto = new ExceptionResponseDto
        {
            ExceptionMessageForUi = message,
            ExceptionMessageForLogging = CreateMessageForLogging(message)
        };

        var serializedExceptionDto = SerializeObject(exceptionDto);

        return serializedExceptionDto;
    }

    private static string CreateMessageForLogging(string message)
    {
        return $"User gets {nameof(InvalidInputCredentialsException)} exception: " +
               $"{message}.";
    }
}