using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.Server;

public class MediatorNotFoundException : Exception, IServerException
{
    public MediatorNotFoundException() : 
        base("IMediator service not found in the dependency injection container.")
    {
        
    }
}