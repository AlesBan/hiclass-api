using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.Database;

public class NotFoundException : Exception, IDbException
{
    public NotFoundException(string entityTitle, object key) :
        base($"Entity {entityTitle} ({key}) was not found.")
    {
    }

    public NotFoundException(string entityTitle, IEnumerable<Guid> keys) :
        base($"Some of these {entityTitle} entities: {string.Join(", ", keys.ToString())}, were not found.")
    {
    }

    public NotFoundException(string entityTitle, object connectionId1, object connectionId2) :
        base($"EntityConnection {entityTitle} " +
             $"(First entityId: {connectionId1}, " +
             $"Second entityId: {connectionId2}) was not found.")
    {
    }
}