using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.User.ResettingPassword;

public class ResetTokenHasExpiredException : Exception, IUiException
{
    public ResetTokenHasExpiredException( Guid userId, string token) :
        base($"Reset token ({token}) of user with {userId} id has expired.")
    {
    }
}