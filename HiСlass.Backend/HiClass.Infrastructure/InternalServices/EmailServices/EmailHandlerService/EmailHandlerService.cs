// EmailHandlerService.cs
using Resend;
using HiClass.Application.Common.Exceptions.Server.EmailManager;
using HiClass.Application.Constants;
using HiClass.Application.Models.EmailManager;
using HiClass.Infrastructure.InternalServices.EmailServices.EmailTemplateService;
using Microsoft.Extensions.Configuration;

namespace HiClass.Infrastructure.InternalServices.EmailServices.EmailHandlerService;

public class EmailHandlerService : IEmailHandlerService
{
    private readonly IResend _resend;
    private readonly IEmailTemplateService _templateService;
    private readonly string _fromEmail;

    public EmailHandlerService(
        IConfiguration configuration,
        IEmailTemplateService templateService,
        IResend resend)
    {
        _templateService = templateService;
        _resend = resend;
        _fromEmail = configuration["EMAIL_RESEND:FROM_EMAIL"] 
                     ?? throw new InvalidOperationException("Resend FromEmail not configured");
    }

    public async Task SendVerificationEmail(string userEmail, string verificationCode)
    {
        var htmlContent = await _templateService.LoadTemplateAsync("EmailVerification", 
            new Dictionary<string, string>
            {
                { "MC:SUBJECT", EmailConstants.EmailVerificationSubject },
                { "verificationCode", verificationCode }
            });

        await SendEmailAsync(userEmail, EmailConstants.EmailVerificationSubject, htmlContent);
    }

    public async Task SendResetPasswordEmail(string userEmail, string resetPasswordCode)
    {
        var htmlContent = await _templateService.LoadTemplateAsync("PasswordReset", 
            new Dictionary<string, string>
            {
                { "MC:SUBJECT", EmailConstants.EmailResetPasswordSubject },
                { "resetPasswordCode", resetPasswordCode }
            });

        await SendEmailAsync(userEmail, EmailConstants.EmailResetPasswordSubject, htmlContent);
    }

    public async Task SendClassInvitationEmail(string senderEmail, string receiverEmail, 
        DateTime invitationDate, string invitationText)
    {
        // Отправка отправителю
        var senderHtml = await _templateService.LoadTemplateAsync("InvitationSent",
            new Dictionary<string, string>
            {
                { "MC:SUBJECT", EmailConstants.EmailInvitationSubject },
                { "receiverEmail", receiverEmail },
                { "invitationDate", invitationDate.ToString("dd.MM.yyyy") },
                { "invitationTime", invitationDate.ToString("HH:mm") },
                { "additionalMessage", "You have sent an invitation" }
            });

        await SendEmailAsync(senderEmail, EmailConstants.EmailInvitationSubject, senderHtml);

        // Отправка получателю
        var receiverHtml = await _templateService.LoadTemplateAsync("InvitationReceived",
            new Dictionary<string, string>
            {
                { "MC:SUBJECT", EmailConstants.EmailInvitationSubject },
                { "senderEmail", senderEmail },
                { "invitationDate", invitationDate.ToString("dd.MM.yyyy") },
                { "invitationTime", invitationDate.ToString("HH:mm") },
                { "additionalMessage", invitationText }
            });

        await SendEmailAsync(receiverEmail, EmailConstants.EmailInvitationSubject, receiverHtml);
    }

    public Task SendExpertInvitationEmail(string senderEmail, string receiverEmail, 
        DateTime invitationDate, string invitationText)
    {
        return SendClassInvitationEmail(senderEmail, receiverEmail, invitationDate, invitationText);
    }

    private async Task SendEmailAsync(string emailReceiver, string subject, string htmlContent)
    {
        try
        {
            var message = new EmailMessage()
            {
                From = _fromEmail,
                To = { emailReceiver },
                Subject = subject,
                HtmlBody = htmlContent
            };

            await _resend.EmailSendAsync(message);
        }
        catch (Exception ex)
        {
            throw new EmailManagerException($"Error sending email: {ex.Message}");
        }
    }
}