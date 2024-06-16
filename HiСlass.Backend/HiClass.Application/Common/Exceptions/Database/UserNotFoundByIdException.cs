using HiClass.Application.Interfaces.Exceptions;
using static Newtonsoft.Json.JsonConvert;


namespace HiClass.Application.Common.Exceptions.Database;

public class UserNotFoundByIdException : Exception, IUiException
{
    private const string ExceptionMessageForUi = "User not found.";

    public UserNotFoundByIdException(Guid userId) : base(CreateSerializedExceptionDto(userId))
    {
    }

    private static string CreateSerializedExceptionDto(Guid userId)
    {
        var exceptionDto = new ExceptionDto
        {
            ExceptionMessageForUi = $"ExceptionMessageForUI: {ExceptionMessageForUi}",
            ExceptionMessageForLogging = $"ExceptionMessageForLogging: {CreateMessageForLogging(userId)}"
        };

        var serializedExceptionDto = SerializeObject(exceptionDto);

        return serializedExceptionDto;
    }

    private static string CreateMessageForLogging(Guid userId)
    {
        return $"{userId} gets {nameof(UserNotFoundByIdException)} exception: " +
               $"User with {userId} id was not found.";
    }
}