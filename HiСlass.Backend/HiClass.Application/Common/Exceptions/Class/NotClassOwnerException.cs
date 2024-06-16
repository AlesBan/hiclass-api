using HiClass.Application.Interfaces.Exceptions;
using static Newtonsoft.Json.JsonConvert;

namespace HiClass.Application.Common.Exceptions.Class;

public class NotClassOwnerException : Exception, IUiException
{
    private const string ExceptionMessageForUi = "User is not the owner of class.";

    public NotClassOwnerException(Guid userId, Guid classId) : 
        base(CreateSerializedExceptionDto(userId, classId))
    {
    }

    private static string CreateSerializedExceptionDto(Guid userId, Guid classId)
    {
        var exceptionDto = new ExceptionDto
        {
            ExceptionMessageForUi = $"ExceptionMessageForUI: {ExceptionMessageForUi}",
            ExceptionMessageForLogging = $"ExceptionMessageForLogging: {CreateMessageForLogging(userId, classId)}"
        };

        var serializedExceptionDto = SerializeObject(exceptionDto);

        return serializedExceptionDto;
    }

    private static string CreateMessageForLogging(Guid userId, Guid classId)
    {
        return $"{userId} gets {nameof(NotClassOwnerException)} exception: " +
               $"User {userId} attempted to delete class {classId} but is not the owner.";
    }
}