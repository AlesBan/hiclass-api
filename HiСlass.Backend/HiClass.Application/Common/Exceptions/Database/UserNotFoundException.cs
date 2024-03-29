using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.Database;

public class UserNotFoundException : Exception, IDbException
{
    public UserNotFoundException(string email) :
        base("User with email " + email + " was not found.")
    {
    }

    public UserNotFoundException(Guid id) :
        base("User with id " + id + " was not found.")
    {
    }
}