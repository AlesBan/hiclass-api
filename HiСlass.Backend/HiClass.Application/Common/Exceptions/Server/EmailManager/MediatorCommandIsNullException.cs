using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.Server.EmailManager;

public class MediatorCommandIsNullException : Exception, IServerException
{
    public MediatorCommandIsNullException() : base("Mediator command is null")
    {
    }
}