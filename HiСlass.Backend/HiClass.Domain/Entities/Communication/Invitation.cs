using HiClass.Domain.Entities.Main;
using HiClass.Domain.Enums;

namespace HiClass.Domain.Entities.Communication;

public class Invitation
{
    public Guid InvitationId { get; set; }
    public Guid UserSenderId { get; set; }
    public User? UserSender { get; set; }
    public Guid UserRecipientId { get; set; }
    public User? UserRecipient { get; set; }
    public Guid? ClassSenderId { get; set; }
    public Class? ClassSender { get; set; }
    public Guid? ClassRecipientId { get; set; }
    public Class? ClassRecipient { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime DateOfInvitation { get; set; }
    public InvitationType Type { get; set; }
    public InvitationStatus Status { get; set; } 
    public string? InvitationText { get; set; } = string.Empty;
    public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
}