using System.ComponentModel.DataAnnotations;
using HiClass.Application.Attributes;

namespace HiClass.Application.Models.Invitations.CreateInvitation;

public class CreateClassInvitationRequestDto
{
    [Required]
    [NotEqual(nameof(ClassRecipientId), nameof(Domain.Entities.Main.Class))]
    public Guid ClassSenderId { get; set; }
    [Required] public Guid ClassRecipientId { get; set; }
    [Required] public string DateOfInvitation { get; set; } = null!;
    [Required] public string? InvitationText { get; set; } = string.Empty;
}