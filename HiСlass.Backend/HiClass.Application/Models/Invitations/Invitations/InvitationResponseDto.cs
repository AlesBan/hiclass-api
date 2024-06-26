using AutoMapper;
using HiClass.Application.Common.Mappings;
using HiClass.Domain.Entities.Communication;

namespace HiClass.Application.Models.Invitations.Invitations;

public class InvitationResponseDto : IMapWith<Invitation>
{
    public string UserSenderEmail { get; set; } = string.Empty;
    public string UserReceiverEmail { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime DateOfInvitation { get; set; }
    public string? InvitationText { get; set; } = string.Empty;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Invitation, InvitationResponseDto>()
            .ForMember(x => x.CreatedAt, opt => opt.MapFrom(x => x.CreatedAt))
            .ForMember(x => x.InvitationText, opt => opt.MapFrom(x => x.InvitationText ?? string.Empty))
            .ForMember(x => x.DateOfInvitation, opt => opt.MapFrom(x => x.DateOfInvitation))
            .ForMember(x => x.UserReceiverEmail, opt => opt.MapFrom(x => x.UserReceiver.Email))
            .ForMember(x => x.UserSenderEmail, opt => opt.MapFrom(x => x.UserSender.Email));
    }
}