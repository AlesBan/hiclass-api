using HiClass.Application.Interfaces.Exceptions;
using static Newtonsoft.Json.JsonConvert;

namespace HiClass.Application.Common.Exceptions.Database;

public class NotFoundException : Exception, IDbException
{
    public NotFoundException(string entityTitle, object key) :
        base(CreateSerializedExceptionDto(entityTitle, key))
    {
    }

    public NotFoundException(string entityTitle, IEnumerable<Guid> keys) :
        base(CreateSerializedExceptionDto(entityTitle, keys))
    {
    }

    public NotFoundException(string entityTitle, object connectionId1, object connectionId2) :
        base(CreateSerializedExceptionDto(entityTitle, connectionId1, connectionId2))
    {
    }

    private static string CreateSerializedExceptionDto(string entityTitle, object key)
    {
        var exceptionDto = new ExceptionResponseDto
        {
            ExceptionMessageForUi = $"Entity {entityTitle} was not found.",
            ExceptionMessageForLogging = CreateMessageForLogging(entityTitle, key)
        };

        var serializedExceptionDto = SerializeObject(exceptionDto);

        return serializedExceptionDto;
    }

    private static string CreateSerializedExceptionDto(string entityTitle, IEnumerable<Guid> keys)
    {
        var exceptionDto = new ExceptionResponseDto
        {
            ExceptionMessageForUi = $"Some of these {entityTitle} entities were not found.",
            ExceptionMessageForLogging = CreateMessageForLogging(entityTitle, keys)
        };

        var serializedExceptionDto = SerializeObject(exceptionDto);

        return serializedExceptionDto;
    }

    private static string CreateSerializedExceptionDto(string entityTitle, object connectionId1, object connectionId2)
    {
        var exceptionDto = new ExceptionResponseDto
        {
            ExceptionMessageForUi = $"EntityConnection {entityTitle} was not found.",
            ExceptionMessageForLogging = CreateMessageForLogging(entityTitle, connectionId1, connectionId2)
        };

        var serializedExceptionDto = SerializeObject(exceptionDto);

        return serializedExceptionDto;
    }

    private static string CreateMessageForLogging(string entityTitle, object key)
    {
        return $"Entity {entityTitle} ({key}) was not found.";
    }

    private static string CreateMessageForLogging(string entityTitle, IEnumerable<Guid> keys)
    {
        return $"Some of these {entityTitle} entities: {string.Join(", ", keys)}, were not found.";
    }

    private static string CreateMessageForLogging(string entityTitle, object connectionId1, object connectionId2)
    {
        return $"EntityConnection {entityTitle} (First entityId: {connectionId1}, Second entityId: {connectionId2}) was not found.";
    }
}
