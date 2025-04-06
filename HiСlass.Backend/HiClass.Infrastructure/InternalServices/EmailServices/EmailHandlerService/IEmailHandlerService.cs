namespace HiClass.Infrastructure.InternalServices.EmailServices.EmailHandlerService;

public interface IEmailHandlerService
{
    Task SendVerificationEmail(string userEmail, string verificationCode);
    Task SendResetPasswordEmail(string userEmail, string resetPasswordCode);
    Task SendClassInvitationEmail(string senderEmail, string receiverEmail, 
        DateTime invitationDate, string invitationText);
    Task SendExpertInvitationEmail(string senderEmail, string receiverEmail, 
        DateTime invitationDate, string invitationText);

}