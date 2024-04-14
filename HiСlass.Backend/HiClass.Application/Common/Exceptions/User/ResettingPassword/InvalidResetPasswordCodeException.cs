using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.User.ResettingPassword;

public class InvalidResetPasswordCodeException : Exception, IUiException
{
    public InvalidResetPasswordCodeException()
        : base("Invalid reset password code")
    {
    }
}