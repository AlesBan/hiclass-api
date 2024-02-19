using System.ComponentModel.DataAnnotations;

namespace HiClass.Application.Models.Invitation.Feedback;

public class SendNotificationForFeedbackRequestOnClientDto
{
    [Required] public Guid InvitationId { get; set; }
    [Required] public Guid UserReceiverId { get; set; }
    [Required] public Guid ClassSenderId { get; set; }
    [Required] public Guid ClassReceiverId { get; set; }
}