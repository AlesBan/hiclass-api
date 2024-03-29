using HiClass.Domain.Entities.Main;

namespace HiClass.Domain.Entities.Communication;

public class Invitation
{
    public Guid InvitationId { get; set; }
    public Guid UserSenderId { get; set; }
    public User UserSender { get; set; }
    public Guid UserReceiverId { get; set; }
    public User UserReceiver { get; set; }
    public Guid ClassSenderId { get; set; }
    public Class ClassSender { get; set; }
    public Guid ClassReceiverId { get; set; }
    public Class ClassReceiver { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime DateOfInvitation { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? InvitationText { get; set; } = string.Empty;
    public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
}