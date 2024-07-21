using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.Device;

public class UserDeviceNotFoundByDeviceTokenException : Exception, IServerException
{
    public UserDeviceNotFoundByDeviceTokenException(string deviceToken) : base(
        $"User device with provided device token {deviceToken} not found!")
    {
    }
}