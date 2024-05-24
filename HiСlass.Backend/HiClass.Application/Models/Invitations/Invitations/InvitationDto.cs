using System.ComponentModel.DataAnnotations;
using AutoMapper;
using HiClass.Application.Common.Mappings;
using HiClass.Application.Models.Invitations.Feedbacks;
using HiClass.Domain.Entities.Communication;

namespace HiClass.Application.Models.Invitations.Invitations;

public class InvitationDto : IMapWith<Invitation>
{
    [Required] public Guid InvitationId { get; set; }
    [Required] public Guid UserSenderId { get; set; }
    [Required] public Guid UserReceiverId { get; set; }
    [Required] public Guid ClassSenderId { get; set; }
    [Required] public Guid ClassReceiverId { get; set; }
    [Required] public DateTime CreatedAt { get; set; }
    [Required] public DateTime DateOfInvitation { get; set; }
    [Required] public string Status { get; set; } = string.Empty;
    [Required] public string? InvitationText { get; set; } = string.Empty;
    [Required] public ICollection<FeedbackDto> Feedbacks { get; set; } = new List<FeedbackDto>();

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Feedback, FeedbackDto>()
            .ForMember(x => x.FeedbackId, opt => opt.MapFrom(x => x.FeedbackId))
            .ForMember(x => x.InvitationId, opt => opt.MapFrom(x => x.InvitationId))
            .ForMember(x => x.UserSenderId, opt => opt.MapFrom(x => x.UserSenderId))
            .ForMember(x => x.UserSenderFullName, opt => opt.MapFrom(x =>
                x.UserSender.FullName))
            .ForMember(x => x.UserSenderImageUrl, opt => opt.MapFrom(x =>
                x.UserSender.ImageUrl))
            .ForMember(x=>x.UserSenderFullLocation, opt => opt.MapFrom(x =>
                x.UserSender.FullLocation))
            .ForMember(x => x.WasTheJointLesson, opt => opt.MapFrom(x => x.WasTheJointLesson))
            .ForMember(x => x.FeedbackText, opt => opt.MapFrom(x => x.FeedbackText))
            .ForMember(x => x.Rating, opt => opt.MapFrom(x => x.Rating))
            .ForMember(x => x.CreatedAt, opt => opt.MapFrom(x => x.CreatedAt));
        profile.CreateMap<Invitation, InvitationDto>()
            .ForMember(x => x.Feedbacks, opt => opt.MapFrom(x => x.Feedbacks))
            .ForMember(x => x.Status, opt => opt.MapFrom(x => x.Status))
            .ForMember(x => x.InvitationText, opt => opt.MapFrom(x => x.InvitationText))
            .ForMember(x => x.CreatedAt, opt => opt.MapFrom(x => x.CreatedAt))
            .ForMember(x => x.DateOfInvitation, opt => opt.MapFrom(x => x.DateOfInvitation))
            .ForMember(x => x.UserSenderId, opt => opt.MapFrom(x => x.UserSenderId))
            .ForMember(x => x.UserReceiverId, opt => opt.MapFrom(x => x.UserReceiverId))
            .ForMember(x => x.ClassSenderId, opt => opt.MapFrom(x => x.ClassSenderId))
            .ForMember(x => x.ClassReceiverId, opt => opt.MapFrom(x => x.ClassReceiverId));
    }
}