using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.User;

public class ProvidedPasswordDoNotMatchesUserPasswordHashException : Exception, IUiException
{
    public ProvidedPasswordDoNotMatchesUserPasswordHashException(string message) : base(message)
    {
    }
}
