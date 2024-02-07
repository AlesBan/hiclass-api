using HiClass.Application.Models.EmailManager;
using Microsoft.Extensions.Configuration;

namespace HiClass.Infrastructure.Services.EmailHandlerService;

public interface IEmailHandlerService
{
    public Task SendVerificationEmail(string userEmail, string verificationCode);
    public Task SendAsync(string emailReceiver, string subject, string message);
    public Task SendResetPasswordEmail(string userEmail, string resetPasswordCode);
}