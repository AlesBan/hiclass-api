using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.Firebase;

public class InvalidFirebaseTokenProvidedException : Exception, IUiException
{
    public InvalidFirebaseTokenProvidedException() : base("Invalid Firebase token provided")
    {
    }
}