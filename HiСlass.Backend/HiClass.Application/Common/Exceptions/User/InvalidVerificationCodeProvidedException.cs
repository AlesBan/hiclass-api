using HiClass.Application.Interfaces.Exceptions;
using static Newtonsoft.Json.JsonConvert;

namespace HiClass.Application.Common.Exceptions.User;

public class InvalidVerificationCodeProvidedException : Exception, IUiException
{
    public InvalidVerificationCodeProvidedException(Guid userId, string enteredCode) :
        base(CreateSerializedExceptionDto(userId, enteredCode))
    {
    }

    private static string CreateSerializedExceptionDto(Guid userId, string enteredCode)
    {
        var exceptionDto = new ExceptionResponseDto
        {
            ExceptionMessageForUi = "Invalid verification code provided.",
            ExceptionMessageForLogging = CreateMessageForLogging(userId, enteredCode)
        };

        var serializedExceptionDto = SerializeObject(exceptionDto);

        return serializedExceptionDto;
    }

    private static string CreateMessageForLogging(Guid userId, string enteredCode)
    {
        return $"{userId} gets {nameof(InvalidVerificationCodeProvidedException)} exception: " +
               $"Invalid verification code provided: {enteredCode}";
    }
}