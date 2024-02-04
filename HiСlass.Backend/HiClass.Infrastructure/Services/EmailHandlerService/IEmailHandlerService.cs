using HiClass.Application.Models.EmailManager;
using Microsoft.Extensions.Configuration;

namespace HiClass.Infrastructure.Services.EmailHandlerService;

public interface IEmailHandlerService
{
    public Task SendVerificationEmail(EmailManagerCredentials credentials, string userEmail, string verificationCode);
    public Task SendAsync(EmailManagerCredentials credentials , string emailReceiver, string subject, string message);
    public Task SendResetPasswordEmail(EmailManagerCredentials credentials, string userEmail, string resetPasswordCode);
}