using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.Authentication;

public class RefreshTokenNotFoundException : Exception, IDbException
{
    public RefreshTokenNotFoundException() : base("Refresh token not found")
    {
    }
}