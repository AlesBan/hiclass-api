using HiClass.Application.Common.Exceptions.Server.EmailManager;
using HiClass.Application.Constants;
using HiClass.Application.Models.EmailManager;
using HiClass.Infrastructure.IntegrationServices.EmailHandlerService;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace HiClass.Infrastructure.InternalServices.EmailHandlerService;

public class EmailHandlerService : IEmailHandlerService
{
    private readonly EmailManagerCredentials _emailCredentials;

    public EmailHandlerService(IConfiguration configuration)
    {
        _emailCredentials = new EmailManagerCredentials(configuration["EMAIL_MANAGER:EMAIL"],
            configuration["EMAIL_MANAGER:PASSWORD"]);
    }

    public async Task SendVerificationEmail(string userEmail,
        string verificationCode)
    {
        var message = EmailConstants.EmailVerificationMessage + verificationCode;
        await SendAsync(userEmail, EmailConstants.EmailVerificationSubject,
            message);
    }

    public async Task SendResetPasswordEmail(string userEmail,
        string resetPasswordCode)
    {
        var message = EmailConstants.EmailResetPasswordMessage + resetPasswordCode;
        await SendAsync(userEmail, EmailConstants.EmailResetPasswordSubject,
            message);
    }

    public async Task SendAsync(string emailReceiver, string subject, string message)
    {
        var email = new MimeMessage();

        var managerEmail = _emailCredentials.Email;
        var managerPassword = _emailCredentials.Password;

        email.From.Add(MailboxAddress.Parse(managerEmail));
        email.To.Add(MailboxAddress.Parse(emailReceiver));

        email.Subject = subject;
        email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };

        using var client = new MailKit.Net.Smtp.SmtpClient();
        await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTlsWhenAvailable);

        try
        {
            await client.AuthenticateAsync(managerEmail, managerPassword);
            await client.SendAsync(email);
        }
        catch (AuthenticationException)
        {
            throw new EmailManagerAuthenticationException();
        }
        catch (Exception)
        {
            throw new EmailManagerException();
        }

        await client.DisconnectAsync(true);
    }
}