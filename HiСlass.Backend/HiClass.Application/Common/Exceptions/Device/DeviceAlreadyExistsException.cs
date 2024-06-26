using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.Device;

public class DeviceAlreadyExistsException : Exception, IUiException
{
    public DeviceAlreadyExistsException() : base("Device already exists.")
    {
    }
}