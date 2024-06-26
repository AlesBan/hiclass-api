using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.Device;

public class DeviceForThisUserIsAlreadyRegisteredException : Exception, IServerException
{
    public DeviceForThisUserIsAlreadyRegisteredException() : base("Device is already exists")
    {
    }
}