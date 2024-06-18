using HiClass.Application.Interfaces.Exceptions;
using static Newtonsoft.Json.JsonConvert;

namespace HiClass.Application.Common.Exceptions.User.Forbidden;

public class UserHasNotAccountException : Exception, IUiForbiddenException
{
    public UserHasNotAccountException(Guid userId) : 
        base(CreateSerializedExceptionDto(userId))
    {
    }

    private static string CreateSerializedExceptionDto(Guid userId)
    {
        var exceptionDto = new ExceptionResponseDto
        {
            ExceptionMessageForUi = "User has not account.",
            ExceptionMessageForLogging = CreateMessageForLogging(userId)
        };

        var serializedExceptionDto = SerializeObject(exceptionDto);

        return serializedExceptionDto;
    }

    private static string CreateMessageForLogging(Guid userId)
    {
        return $"User {userId} gets {nameof(UserHasNotAccountException)} exception: User has not account.";
    }
}