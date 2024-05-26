using System.ComponentModel.DataAnnotations;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.InvitationHandlers.Commands.ChangeInvitationStatus;

public class ChangeInvitationStatusCommand : IRequest
{
    [Required] public Guid InvitationId { get; set; }
    [Required] public Guid UserReceiverId { get; set; }
    [Required] public bool IsAccepted { get; set; }
}