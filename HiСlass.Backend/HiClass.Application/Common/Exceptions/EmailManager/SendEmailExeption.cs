using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.EmailManager;

public class SendEmailException : Exception, IServerException
{
    public SendEmailException() : base("Failed to send email.")
    {
    }
}