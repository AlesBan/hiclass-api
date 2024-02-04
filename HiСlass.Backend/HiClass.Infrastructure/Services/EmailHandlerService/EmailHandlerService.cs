using HiClass.Application.Common.Exceptions.EmailManager;
using HiClass.Application.Constants;
using HiClass.Application.Models.EmailManager;
using MailKit.Security;
using MimeKit;

namespace HiClass.Infrastructure.Services.EmailHandlerService;

public class EmailHandlerService : IEmailHandlerService
{
    public async Task SendVerificationEmail(EmailManagerCredentials credentials, string userEmail,
        string verificationCode)
    {
        var message = EmailConstants.EmailConfirmationMessage + verificationCode;
        await SendAsync(credentials, userEmail,
            EmailConstants.EmailConfirmationSubject,
            message);
    }

    public async Task SendResetPasswordEmail(EmailManagerCredentials credentials, string userEmail,
        string resetPasswordCode)
    {
        var message = EmailConstants.EmailResetPasswordMessage + resetPasswordCode;
        await SendAsync(credentials, userEmail,
            EmailConstants.EmailResetPasswordSubject,
            message);
    }

    public async Task SendAsync(EmailManagerCredentials credentials, string emailReceiver, string subject,
        string message)
    {
        var email = new MimeMessage();

        var managerEmail = credentials.Email;
        var managerPassword = credentials.Email;

        email.From.Add(MailboxAddress.Parse(managerEmail));
        email.To.Add(MailboxAddress.Parse(emailReceiver));

        email.Subject = subject;
        email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };

        using var client = new MailKit.Net.Smtp.SmtpClient();
        await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

        try
        {
            await client.AuthenticateAsync(managerEmail, managerPassword);
            await client.SendAsync(email);
        }
        catch (AuthenticationException e)
        {
            throw new EmailManagerAuthenticationException();
        }
        catch (Exception e)
        {
            throw new SendEmailException();
        }

        await client.DisconnectAsync(true);
    }
}