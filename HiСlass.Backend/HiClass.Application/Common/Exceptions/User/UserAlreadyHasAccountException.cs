using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.User;

public class UserAlreadyHasAccountException : Exception, IUiException
{
    public UserAlreadyHasAccountException(Guid userId) : base($"User {userId} already has account")
    {
    }
}