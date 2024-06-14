using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.Database;

public class NotFoundException : Exception, IDbException
{
    /// <summary>
    /// NotFoundException entity
    /// </summary>
    /// <param name="entityTitle"></param>
    /// <param name="key"></param>
    public NotFoundException(string entityTitle, object key) :
        base($"Entity {entityTitle} ({key}) was not found.")
    {
    }

    /// <summary>
    /// NotFoundException entity
    /// </summary>
    /// <param name="entityTitle"></param>
    /// <param name="keys"></param>
    public NotFoundException(string entityTitle, IEnumerable<Guid> keys) :
        base($"Some of these {entityTitle} entities: {string.Join(", ", keys.ToString())}, were not found.")
    {
    }

    /// <summary>
    /// NotFoundException connection
    /// </summary>
    /// <param name="entityTitle"></param>
    /// <param name="connectionId1"></param>
    /// <param name="connectionId2"></param>
    public NotFoundException(string entityTitle, object connectionId1, object connectionId2) :
        base($"EntityConnection {entityTitle} " +
             $"(First entityId: {connectionId1}, " +
             $"Second entityId: {connectionId2}) was not found.")
    {
    }
}