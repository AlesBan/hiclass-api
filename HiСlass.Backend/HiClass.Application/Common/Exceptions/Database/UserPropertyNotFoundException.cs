using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.Database;

public class UserPropertyNotFoundException : Exception, IDbException
{
    public UserPropertyNotFoundException(Guid userId, string propertyName) :
        base("User property " + propertyName + " with " + userId + "id was not found.")
    {
    }
}