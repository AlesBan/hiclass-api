using System.ComponentModel.DataAnnotations;
using HiClass.Application.Attributes;
using HiClass.Domain.Entities.Main;

namespace HiClass.Application.Dtos.InvitationDtos;

public class CreateInvitationRequestDto
{
    [Required]
    [NotEqual(nameof(ClassReceiverId), nameof(Class))]
    public Guid ClassSenderId { get; set; }
    [Required] public Guid ClassReceiverId { get; set; }
    [Required] public string DateOfInvitation { get; set; }
    [Required] public string? InvitationText { get; set; } = string.Empty;
}