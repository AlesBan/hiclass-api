using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.Device;

public class DeviceByTokenNotFoundException : Exception, IServerException
{
    public DeviceByTokenNotFoundException(string deviceToken) : base(
        $"Device with provided token {deviceToken} not found!")
    {
    }
}