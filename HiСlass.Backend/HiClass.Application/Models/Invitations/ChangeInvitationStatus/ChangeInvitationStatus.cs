namespace HiClass.Application.Models.Invitations.ChangeInvitationStatus;

public class ChangeInvitationStatusRequestDto
{
    public Guid InvitationId { get; set; }
    public bool IsAccepted { get; set; }
}