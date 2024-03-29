using System.ComponentModel.DataAnnotations;
using HiClass.Application.Attributes;

namespace HiClass.Application.Models.Invitations.CreateInvitation;

public class CreateInvitationRequestDto
{
    [Required]
    [NotEqual(nameof(ClassReceiverId), nameof(Domain.Entities.Main.Class))]
    public Guid ClassSenderId { get; set; }
    [Required] public Guid ClassReceiverId { get; set; }
    [Required] public string DateOfInvitation { get; set; } = null!;
    [Required] public string? InvitationText { get; set; } = string.Empty;
}