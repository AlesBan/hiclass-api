using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.Server.EmailManager;

public class EmailManagerAuthenticationException : Exception, IServerException
{
    public EmailManagerAuthenticationException() : base("Failed to authenticate email manager.")
    {
        
    }
}