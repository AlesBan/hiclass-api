namespace HiClass.Application.Interfaces.Services;

public interface IEmailHandlerService
{
    public Task SendVerificationEmail(string userEmail, string verificationCode);
    public Task SendAsync(string emailReceiver, string subject, string message);
    public Task SendResetPasswordEmail(string userEmail, string resetPasswordCode);
}