using HiClass.Application.Interfaces.Exceptions;
using static Newtonsoft.Json.JsonConvert;

namespace HiClass.Application.Common.Exceptions.User.Forbidden;

public class UserNotVerifiedException : Exception, IUiForbiddenException
{
    public UserNotVerifiedException(Guid userId) : 
        base(CreateSerializedExceptionDto(userId))
    {
    }

    private static string CreateSerializedExceptionDto(Guid userId)
    {
        var exceptionDto = new ExceptionResponseDto
        {
            ExceptionMessageForUi = "User is not verified.",
            ExceptionMessageForLogging = CreateMessageForLogging(userId)
        };

        var serializedExceptionDto = SerializeObject(exceptionDto);

        return serializedExceptionDto;
    }

    private static string CreateMessageForLogging(Guid userId)
    {
        return $"User {userId} gets {nameof(UserNotVerifiedException)} exception: User is not verified.";
    }
}