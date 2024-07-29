using HiClass.Domain.Entities.Main;

namespace HiClass.Domain.Entities.Communication;

public class Feedback
{
    public Guid FeedbackId { get; set; }
    public Guid InvitationId { get; set; }
    public Invitation? Invitation { get; set; }
    public Guid UserFeedbackSenderId { get; set; }
    public User? UserFeedbackSender { get; set; }
    public Guid UserFeedbackReceiverId { get; set; }
    public User? UserFeedbackReceiver { get; set; }
    public bool WasTheJointLesson { get; set; }
    public string? ReasonForNotConducting { get; set; }
    public string? FeedbackText { get; set; }
    public int? Rating { get; set; }
    public DateTime CreatedAt { get; set; }
}