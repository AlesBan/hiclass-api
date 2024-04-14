using System.ComponentModel.DataAnnotations;
using HiClass.Application.Attributes;

namespace HiClass.Application.Models.Invitations.Feedbacks.CreateFeedback;

public class CreateFeedbackRequestDto
{
    [Required] public Guid UserReceiverId { get; set; }

    [NotEqual(nameof(ClassSenderId), nameof(Domain.Entities.Main.Class))]
    [Required]
    public Guid ClassReceiverId { get; set; }

    [Required] public Guid ClassSenderId { get; set; }
    [Required] public Guid InvitationId { get; set; }
    [Required] public int Rating { get; set; }
    [Required] public string FeedbackText { get; set; } = string.Empty;
    [Required] public bool WasTheJointLesson { get; set; }
    public string? ReasonForNotConducting { get; set; }
}