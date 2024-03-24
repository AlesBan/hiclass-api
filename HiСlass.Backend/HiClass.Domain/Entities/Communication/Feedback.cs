using HiClass.Domain.Entities.Main;

namespace HiClass.Domain.Entities.Communication;

public class Feedback
{
    public Guid FeedbackId { get; set; }
    public Guid InvitationId { get; set; }
    public Invitation Invitation { get; set; }
    public Guid UserSenderId { get; set; }
    public User UserSender { get; set; }
    public Guid UserRecipientId { get; set; }
    public User UserRecipient { get; set; }
    public Guid ClassSenderId { get; set; }
    public Class ClassSender { get; set; }
    public Guid ClassReceiverId { get; set; }
    public Class ClassReceiver { get; set; }
    public bool WasTheJointLesson { get; set; }
    public string? ReasonForNotConducting { get; set; }
    public string? FeedbackText { get; set; }
    public int? Rating { get; set; }
    // public DateTime CreatedAt { get; set; }
}