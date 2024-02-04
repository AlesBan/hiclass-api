using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.EmailManager;

public class EmailManagerAuthenticationException : Exception, IServerException
{
    public EmailManagerAuthenticationException() : base("Failed to authenticate email manager.")
    {
        
    }
}