using HiClass.Domain.Enums;

namespace HiClass.Application.Models.Invitations.UpdateInvitationStatus;

public class UpdateInvitationStatusRequestDto
{
    public Guid InvitationId { get; set; }
    public InvitationStatus Status { get; set; }
}