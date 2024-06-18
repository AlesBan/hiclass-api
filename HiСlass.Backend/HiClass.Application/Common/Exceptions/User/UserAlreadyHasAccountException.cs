using HiClass.Application.Interfaces.Exceptions;
using static Newtonsoft.Json.JsonConvert;

namespace HiClass.Application.Common.Exceptions.User;

public class UserAlreadyHasAccountException : Exception, IUiException
{
    public UserAlreadyHasAccountException(Guid userId) :
        base(CreateSerializedExceptionDto(userId))
    {
    }

    private static string CreateSerializedExceptionDto(Guid userId)
    {
        var exceptionDto = new ExceptionResponseDto
        {
            ExceptionMessageForUi = "User already has an account.",
            ExceptionMessageForLogging = CreateMessageForLogging(userId)
        };

        var serializedExceptionDto = SerializeObject(exceptionDto);

        return serializedExceptionDto;
    }

    private static string CreateMessageForLogging(Guid userId)
    {
        return $"{userId} gets {nameof(UserAlreadyHasAccountException)} exception: User already has an account.";
    }
}