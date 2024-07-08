using System.ComponentModel.DataAnnotations;
using HiClass.Domain.Enums;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.InvitationHandlers.Commands.ChangeInvitationStatus;

public class UpdateInvitationStatusCommand : IRequest
{
    [Required] public Guid InvitationId { get; set; }
    [Required] public Guid UserId { get; set; }
    [Required] public InvitationStatus Status { get; set; }
}