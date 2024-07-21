using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.Device;

public class DeviceByIdNotFoundException: Exception, IServerException
{
    
    public DeviceByIdNotFoundException(Guid id) : base($"Device with provided id {id} not found!")
    {
    }
}