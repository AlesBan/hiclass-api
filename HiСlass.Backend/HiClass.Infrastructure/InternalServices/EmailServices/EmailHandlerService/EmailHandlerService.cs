using HiClass.Application.Common.Exceptions.Server.EmailManager;
using HiClass.Application.Constants;
using HiClass.Application.Models.EmailManager;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using HiClass.Infrastructure.InternalServices.EmailServices.EmailTemplateService;

namespace HiClass.Infrastructure.InternalServices.EmailServices.EmailHandlerService;

public class EmailHandlerService : IEmailHandlerService
{
    private readonly EmailManagerCredentials _emailCredentials;
    private readonly IEmailTemplateService _templateService;

    public EmailHandlerService(
        IConfiguration configuration,
        IEmailTemplateService templateService)
    {
        _emailCredentials = new EmailManagerCredentials(
            configuration["EMAIL_MANAGER:EMAIL"],
            configuration["EMAIL_MANAGER:PASSWORD"]);
        _templateService = templateService;
    }

    public async Task SendVerificationEmail(string userEmail, string verificationCode)
    {
        var htmlContent = await _templateService.LoadTemplateAsync("EmailVerification", new Dictionary<string, string>
        {
            {"MC:SUBJECT", EmailConstants.EmailVerificationSubject},
            {"verificationCode", verificationCode}
        });

        await SendHtmlEmailAsync(userEmail, EmailConstants.EmailVerificationSubject, htmlContent);
    }

    public async Task SendResetPasswordEmail(string userEmail, string resetPasswordCode)
    {
        var htmlContent = await _templateService.LoadTemplateAsync("PasswordReset", new Dictionary<string, string>
        {
            {"MC:SUBJECT", EmailConstants.EmailResetPasswordSubject},
            {"resetPasswordCode", resetPasswordCode}
        });

        await SendHtmlEmailAsync(userEmail, EmailConstants.EmailResetPasswordSubject, htmlContent);
    }

    public async Task SendClassInvitationEmail(string senderEmail, string receiverEmail, DateTime invitationDate, string invitationText)
    {
        // Письмо отправителю
        var senderHtml = await _templateService.LoadTemplateAsync("InvitationSent", new Dictionary<string, string>
        {
            {"MC:SUBJECT", EmailConstants.EmailInvitationSubject},
            {"receiverEmail", receiverEmail},
            {"invitationDate", invitationDate.ToString("dd.MM.yyyy")},
            {"invitationTime", invitationDate.ToString("HH:mm")},
            {"additionalMessage", "You have sent an invitation"}
        });

        // Письмо получателю
        var receiverHtml = await _templateService.LoadTemplateAsync("InvitationReceived", new Dictionary<string, string>
        {
            {"MC:SUBJECT", EmailConstants.EmailInvitationSubject},
            {"senderEmail", senderEmail},
            {"invitationDate", invitationDate.ToString("dd.MM.yyyy")},
            {"invitationTime", invitationDate.ToString("HH:mm")},
            {"additionalMessage", invitationText}
        });

        await SendHtmlEmailAsync(senderEmail, EmailConstants.EmailInvitationSubject, senderHtml);
        await SendHtmlEmailAsync(receiverEmail, EmailConstants.EmailInvitationSubject, receiverHtml);
    }

    public async Task SendExpertInvitationEmail(string senderEmail, string receiverEmail, DateTime invitationDate, string invitationText)
    {
        // Аналогично SendClassInvitationEmail, но с другим текстом если нужно
        await SendClassInvitationEmail(senderEmail, receiverEmail, invitationDate, invitationText);
    }

    private async Task SendHtmlEmailAsync(string emailReceiver, string subject, string htmlContent)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_emailCredentials.Email));
        email.To.Add(MailboxAddress.Parse(emailReceiver));
        email.Subject = subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = htmlContent,
            TextBody = "Please view this email in an HTML-compatible email client."
        };
        
        email.Body = bodyBuilder.ToMessageBody();

        using var client = new MailKit.Net.Smtp.SmtpClient();
        try
        {
            await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTlsWhenAvailable);
            await client.AuthenticateAsync(_emailCredentials.Email, _emailCredentials.Password);
            await client.SendAsync(email);
        }
        catch (AuthenticationException)
        {
            throw new EmailManagerAuthenticationException();
        }
        catch (Exception ex)
        {
            // Логирование ошибки может быть добавлено здесь
            throw new EmailManagerException();
        }
        finally
        {
            await client.DisconnectAsync(true);
        }
    }
}