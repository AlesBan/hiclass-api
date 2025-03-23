using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.Device;

public class UserDeviceNotFoundByRefreshTokenException : Exception, IServerException
{
    public UserDeviceNotFoundByRefreshTokenException(string refreshToken) :
        base($"User device with provided refresh token {refreshToken} not found!")
    {
    }
}