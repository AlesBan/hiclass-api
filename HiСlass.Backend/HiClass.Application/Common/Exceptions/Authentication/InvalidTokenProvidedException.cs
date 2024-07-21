using HiClass.Application.Interfaces.Exceptions;
using static Newtonsoft.Json.JsonConvert;


namespace HiClass.Application.Common.Exceptions.Authentication;

public class InvalidTokenProvidedException : Exception, IUiException
{
    private const string ExceptionMessageForUi = "Invalid access or refresh token provided.";

    public InvalidTokenProvidedException(string token) :
        base(CreateSerializedExceptionDto(token))
    {
    }

    private static string CreateSerializedExceptionDto(string token)
    {
        var exceptionDto = new ExceptionResponseDto
        {
            ExceptionMessageForUi = ExceptionMessageForUi,
            ExceptionMessageForLogging = CreateMessageForLogging(token)
        };

        var serializedExceptionDto = SerializeObject(exceptionDto);

        return serializedExceptionDto;
    }

    private static string CreateMessageForLogging(string token)
    {
        return $"Invalid access or refresh token provided: {token}";
    }
}