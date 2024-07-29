using System.ComponentModel.DataAnnotations;
using HiClass.Application.Attributes;

namespace HiClass.Application.Models.Invitations.Feedbacks.CreateFeedback;

public class CreateFeedbackRequestDto
{
    [Required] public Guid InvitationId { get; set; }
    [Required] public int Rating { get; set; }
    [Required] public string FeedbackText { get; set; } = string.Empty;
    [Required] public bool WasTheJointLesson { get; set; }
    public string? ReasonForNotConducting { get; set; }
}