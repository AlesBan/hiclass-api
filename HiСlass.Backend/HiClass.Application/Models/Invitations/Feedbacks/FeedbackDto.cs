using System.ComponentModel.DataAnnotations;

namespace HiClass.Application.Models.Invitations.Feedbacks;

public class FeedbackDto
{
    [Required] public Guid FeedbackId { get; set; }
    [Required] public Guid InvitationId { get; set; }
    [Required] public Guid UserSenderId { get; set; }
    [Required] public string UserSenderFullName { get; set; } = null!;
    [Required] public string UserSenderImageUrl { get; set; } = null!;
    [Required] public string UserSenderCityLocationTitle { get; set; } = null!;
    [Required] public bool WasTheJointLesson { get; set; }
    [Required] public string? FeedbackText { get; set; }
    [Required] public int? Rating { get; set; }
    [Required] public DateTime CreatedAt { get; set; }
}