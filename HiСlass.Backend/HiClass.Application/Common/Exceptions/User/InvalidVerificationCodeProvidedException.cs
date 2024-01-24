using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.User;

public class InvalidVerificationCodeProvidedException : Exception, IUiException
{
    public InvalidVerificationCodeProvidedException() :
        base("Invalid verification code provided.")
    {
    }
}