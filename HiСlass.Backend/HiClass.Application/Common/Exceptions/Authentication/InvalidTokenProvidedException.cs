using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.Authentication;

public class InvalidTokenProvidedException : Exception, IUiException
{
    public InvalidTokenProvidedException() : 
        base("Invalid NameIdentifier claim")
    { 
        
    }  
}