using HiClass.Application.Interfaces.Exceptions;
using static Newtonsoft.Json.JsonConvert;

namespace HiClass.Application.Common.Exceptions.User.ResettingPassword;

public class ResetTokenHasExpiredException : Exception, IUiException
{
    public ResetTokenHasExpiredException(Guid userId, string token) :
        base(CreateSerializedExceptionDto(userId, token))
    {
    }

    private static string CreateSerializedExceptionDto(Guid userId, string token)
    {
        var exceptionDto = new ExceptionResponseDto
        {
            ExceptionMessageForUi = "Reset token has expired.",
            ExceptionMessageForLogging = CreateMessageForLogging(userId, token)
        };

        var serializedExceptionDto = SerializeObject(exceptionDto);

        return serializedExceptionDto;
    }

    private static string CreateMessageForLogging(Guid userId, string token)
    {
        return $"{userId} gets {nameof(ResetTokenHasExpiredException)} exception: " +
               $"Reset token has expired: {token}";
    }
}