// EmailHandlerService.cs
using Hangfire;
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
    private readonly IBackgroundJobClient _backgroundJob;

    public EmailHandlerService(
        IConfiguration configuration,
        IEmailTemplateService templateService,
        IBackgroundJobClient backgroundJob)
    {
        _emailCredentials = new EmailManagerCredentials(
            configuration["EMAIL_MANAGER:EMAIL"],
            configuration["EMAIL_MANAGER:PASSWORD"]);
        _templateService = templateService;
        _backgroundJob = backgroundJob;
    }

    public Task SendVerificationEmail(string userEmail, string verificationCode)
    {
        return QueueEmailAsync(userEmail, "EmailVerification", EmailConstants.EmailVerificationSubject, 
            new Dictionary<string, string>
            {
                { "MC:SUBJECT", EmailConstants.EmailVerificationSubject },
                { "verificationCode", verificationCode }
            });
    }

    public Task SendResetPasswordEmail(string userEmail, string resetPasswordCode)
    {
        return QueueEmailAsync(userEmail, "PasswordReset", EmailConstants.EmailResetPasswordSubject,
            new Dictionary<string, string>
            {
                { "MC:SUBJECT", EmailConstants.EmailResetPasswordSubject },
                { "resetPasswordCode", resetPasswordCode }
            });
    }

    public async Task SendClassInvitationEmail(string senderEmail, string receiverEmail, 
        DateTime invitationDate, string invitationText)
    {
        await QueueEmailAsync(senderEmail, "InvitationSent", EmailConstants.EmailInvitationSubject,
            new Dictionary<string, string>
            {
                { "MC:SUBJECT", EmailConstants.EmailInvitationSubject },
                { "receiverEmail", receiverEmail },
                { "invitationDate", invitationDate.ToString("dd.MM.yyyy") },
                { "invitationTime", invitationDate.ToString("HH:mm") },
                { "additionalMessage", "You have sent an invitation" }
            });

        await QueueEmailAsync(receiverEmail, "InvitationReceived", EmailConstants.EmailInvitationSubject,
            new Dictionary<string, string>
            {
                { "MC:SUBJECT", EmailConstants.EmailInvitationSubject },
                { "senderEmail", senderEmail },
                { "invitationDate", invitationDate.ToString("dd.MM.yyyy") },
                { "invitationTime", invitationDate.ToString("HH:mm") },
                { "additionalMessage", invitationText }
            });
    }

    public Task SendExpertInvitationEmail(string senderEmail, string receiverEmail, 
        DateTime invitationDate, string invitationText)
    {
        return SendClassInvitationEmail(senderEmail, receiverEmail, invitationDate, invitationText);
    }

    private async Task QueueEmailAsync(string emailReceiver, string templateName, 
        string subject, Dictionary<string, string> replacements)
    {
        var htmlContent = await _templateService.LoadTemplateAsync(templateName, replacements);
        
        _backgroundJob.Enqueue(() => 
            SendEmailBackgroundJob(emailReceiver, subject, htmlContent));
    }

    [AutomaticRetry(Attempts = 3, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
    public async Task SendEmailBackgroundJob(string emailReceiver, string subject, string htmlContent)
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
            throw new EmailManagerException($"Error sending email: {ex.Message}");
        }
        finally
        {
            await client.DisconnectAsync(true);
        }
    }
}