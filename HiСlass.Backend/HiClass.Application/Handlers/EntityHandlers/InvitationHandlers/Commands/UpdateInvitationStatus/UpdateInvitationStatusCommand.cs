using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.InvitationHandlers.Commands.UpdateInvitationStatus;

public class UpdateInvitationStatusCommand : IRequest
{
    public Guid InvitationId { get; set; }
    public bool IsAccepted { get; set; }
}