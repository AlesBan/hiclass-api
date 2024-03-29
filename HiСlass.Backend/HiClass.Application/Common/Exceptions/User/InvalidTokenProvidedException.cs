using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.User;

public class InvalidTokenProvidedException : Exception, IUiException
{
    public InvalidTokenProvidedException(string token) : base($"Invalid token ({token}) provided.")
    {
    }
}