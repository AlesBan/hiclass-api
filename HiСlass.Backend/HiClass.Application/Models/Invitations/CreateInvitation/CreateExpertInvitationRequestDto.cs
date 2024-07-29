using System.ComponentModel.DataAnnotations;

namespace HiClass.Application.Models.Invitations.CreateInvitation;

public class CreateExpertInvitationRequestDto
{
    [Required] public Guid ClassSenderId { get; set; }
    [Required] public Guid UserRecipientId { get; set; }
    [Required] public string DateOfInvitation { get; set; } = null!;
    [Required] public string? InvitationText { get; set; } = string.Empty;
}