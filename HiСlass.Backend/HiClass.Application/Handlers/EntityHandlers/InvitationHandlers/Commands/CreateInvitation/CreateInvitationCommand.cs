using System.ComponentModel.DataAnnotations;
using HiClass.Application.Attributes;
using HiClass.Domain.Entities.Communication;
using HiClass.Domain.Entities.Main;
using HiClass.Domain.Enums;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.InvitationHandlers.Commands.CreateInvitation;

public class CreateInvitationCommand : IRequest<Invitation>
{
    [Required]
    [NotEqual(nameof(UserReceiverId), nameof(User))]
    public Guid UserSenderId { get; set; }

    [Required] public Guid UserReceiverId { get; set; }

    [Required]
    [NotEqual(nameof(ClassReceiverId), nameof(Class))]
    public Guid ClassSenderId { get; set; }

    [Required] public Guid ClassReceiverId { get; set; }
    [Required] public DateTime DateOfInvitation { get; set; }
    [Required] public InvitationStatus Status { get; set; } = InvitationStatus.Pending;
    [Required] public string? InvitationText { get; set; }
}