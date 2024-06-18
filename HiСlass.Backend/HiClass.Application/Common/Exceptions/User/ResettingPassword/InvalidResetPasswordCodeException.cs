using HiClass.Application.Interfaces.Exceptions;
using static Newtonsoft.Json.JsonConvert;

namespace HiClass.Application.Common.Exceptions.User.ResettingPassword;

public class InvalidResetPasswordCodeException : Exception, IUiException
{
    public InvalidResetPasswordCodeException(Guid userId, string enteredResetPasswordCode) :
        base(CreateSerializedExceptionDto(userId, enteredResetPasswordCode))
    {
    }

    private static string CreateSerializedExceptionDto(Guid userId, string enteredResetPasswordCode)
    {
        var exceptionDto = new ExceptionResponseDto
        {
            ExceptionMessageForUi = "Invalid reset password code.",
            ExceptionMessageForLogging = CreateMessageForLogging(userId, enteredResetPasswordCode)
        };

        var serializedExceptionDto = SerializeObject(exceptionDto);

        return serializedExceptionDto;
    }

    private static string CreateMessageForLogging(Guid userId, string enteredResetPasswordCode)
    {
        return $"{userId} gets {nameof(InvalidResetPasswordCodeException)} exception: " +
               $"Invalid reset password code provided: {enteredResetPasswordCode}";
    }
}