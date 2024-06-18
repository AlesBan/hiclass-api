using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.Server.EmailManager;

public class EmailManagerException : Exception, IServerException
{
    public EmailManagerException() : base("Failed to send email.")
    {
    }
}