using HiClass.Domain.Entities.Communication;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.InvitationHandlers.Commands.DeleteInvitation;

public class DeleteInvitationCommand : IRequest<Unit>
{
    public Invitation Invitation { get; set; }
}