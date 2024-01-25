using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.User;

public class UserAlreadyExistsException : Exception, IUiException
{
    public UserAlreadyExistsException(string email) :
        base("User with email " + email + " already exists")
    {
    }
}