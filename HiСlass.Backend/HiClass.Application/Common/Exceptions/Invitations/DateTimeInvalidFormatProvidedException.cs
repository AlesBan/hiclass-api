using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.Invitations;

public class DateTimeInvalidFormatProvidedException : Exception, IUiException
{
    private const string ExceptionMessageForUi = "Invalid date-time format provided.";

    public DateTimeInvalidFormatProvidedException(Guid userId) : base(CreateSerializedExceptionDto(userId))
    {
    }

    private static string CreateSerializedExceptionDto(Guid userId)
    {
        var exceptionDto = new ExceptionResponseDto
        {
            ExceptionMessageForUi = ExceptionMessageForUi,
            ExceptionMessageForLogging = CreateMessageForLogging(userId)
        };

        var serializedExceptionDto = Newtonsoft.Json.JsonConvert.SerializeObject(exceptionDto);

        return serializedExceptionDto;
    }

    private static string CreateMessageForLogging(Guid userId)
    {
        return
            $"User with {userId} id gets {nameof(DateTimeInvalidFormatProvidedException)} exception: " +
            $"Invalid access token provided";
    }
}