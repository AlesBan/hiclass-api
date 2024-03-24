using System.ComponentModel.DataAnnotations;

namespace HiClass.Application.Models.Invitations.CreateInvitation;

public class CreateInvitationResponseDto
{
    public CreateInvitationResponseDto(Domain.Entities.Communication.Invitation invitation)
    {
        Invitation = invitation;
    }

    [Required] public Domain.Entities.Communication.Invitation Invitation { get; set; }
    
    
}