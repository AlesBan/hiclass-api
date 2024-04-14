using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.User.ResettingPassword;

public class InvalidResetTokenProvidedException: Exception, IUiException
{
    public InvalidResetTokenProvidedException() : base("Invalid reset token provided")
    {
    }
}