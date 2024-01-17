using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions;

public class UnknownTypeException : Exception, IServerException
{
    public UnknownTypeException() : base("Unknown type.")
    {
        
    }
}