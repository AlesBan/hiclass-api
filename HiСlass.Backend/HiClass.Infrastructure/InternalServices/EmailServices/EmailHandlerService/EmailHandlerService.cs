using Resend;
using HiClass.Application.Constants;
using HiClass.Infrastructure.InternalServices.EmailServices.EmailTemplateService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace HiClass.Infrastructure.InternalServices.EmailServices.EmailHandlerService;

public class EmailHandlerService : IEmailHandlerService
{
    private readonly IResend _resend;
    private readonly IEmailTemplateService _templateService;
    private readonly string _fromEmail;
    private readonly ILogger<EmailHandlerService> _logger;
    private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);

    public EmailHandlerService(
        IConfiguration configuration,
        IEmailTemplateService templateService,
        IResend resend,
        ILogger<EmailHandlerService> logger)
    {
        _templateService = templateService ?? throw new ArgumentNullException(nameof(templateService));
        _resend = resend ?? throw new ArgumentNullException(nameof(resend));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        
        _fromEmail = configuration["EMAIL_RESEND:FROM_EMAIL"] 
                   ?? throw new InvalidOperationException("Resend FromEmail not configured in configuration");
    }

    public async Task SendVerificationEmail(string userEmail, string verificationCode)
    {
        await SendEmailWithTemplateAsync(
            userEmail,
            EmailConstants.EmailVerificationSubject,
            "EmailVerification",
            new Dictionary<string, string>
            {
                { "MC:SUBJECT", EmailConstants.EmailVerificationSubject },
                { "verificationCode", verificationCode }
            });
    }

    public async Task SendResetPasswordEmail(string userEmail, string resetPasswordCode)
    {
        await SendEmailWithTemplateAsync(
            userEmail,
            EmailConstants.EmailResetPasswordSubject,
            "PasswordReset",
            new Dictionary<string, string>
            {
                { "MC:SUBJECT", EmailConstants.EmailResetPasswordSubject },
                { "resetPasswordCode", resetPasswordCode }
            });
    }

    public async Task SendClassInvitationEmail(string senderEmail, string receiverEmail, 
        DateTime invitationDate, string invitationText)
    {
  
        await SendEmailWithTemplateAsync(
            senderEmail,
            EmailConstants.EmailInvitationSubject,
            "InvitationSent",
            new Dictionary<string, string>
            {
                { "MC:SUBJECT", EmailConstants.EmailInvitationSubject },
                { "receiverEmail", receiverEmail },
                { "invitationDate", invitationDate.ToString("dd.MM.yyyy") },
                { "invitationTime", invitationDate.ToString("HH:mm") },
                { "additionalMessage", "You have sent an invitation" }
            });

      
        await SendEmailWithTemplateAsync(
            receiverEmail,
            EmailConstants.EmailInvitationSubject,
            "InvitationReceived",
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

    private async Task SendEmailWithTemplateAsync(
        string email,
        string subject,
        string templateName,
        Dictionary<string, string> templateParameters)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogWarning("Attempt to send email to empty address. Template: {Template}", templateName);
                return;
            }

            if (!EmailRegex.IsMatch(email))
            {
                _logger.LogWarning("Invalid email format: {Email} (Template: {Template})", email, templateName);
                return;
            }

            var htmlContent = await _templateService.LoadTemplateAsync(templateName, templateParameters);
            await SendEmailAsync(email, subject, htmlContent);
            
            _logger.LogInformation("Email sent successfully. To: {Email}, Subject: {Subject}, Template: {Template}", 
                email, subject, templateName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email. Template: {Template}, Email: {Email}", 
                templateName, email);
        }
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
            _logger.LogError(ex, "Email sending failed. Address: {Email}, Subject: {Subject}", 
                emailReceiver, subject);
            throw; 
        }
    }
}