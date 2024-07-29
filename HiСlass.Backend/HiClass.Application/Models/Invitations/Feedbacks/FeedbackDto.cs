using System.ComponentModel.DataAnnotations;
using AutoMapper;
using HiClass.Application.Common.Mappings;
using HiClass.Domain.Entities.Communication;

namespace HiClass.Application.Models.Invitations.Feedbacks;

public class FeedbackDto : IMapWith<Feedback>
{
    [Required] public Guid FeedbackId { get; set; }
    [Required] public Guid InvitationId { get; set; }
    [Required] public Guid UserSenderId { get; set; }
    [Required] public string UserSenderFullName { get; set; } = null!;
    [Required] public string UserSenderImageUrl { get; set; } = null!;
    [Required] public string UserSenderFullLocation { get; set; } = null!;
    [Required] public bool WasTheJointLesson { get; set; }
    [Required] public string? FeedbackText { get; set; }
    [Required] public int? Rating { get; set; }
    [Required] public DateTime CreatedAt { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<Feedback, FeedbackDto>()
            .ForMember(x => x.FeedbackId, opt => opt.MapFrom(x => x.FeedbackId))
            .ForMember(x => x.InvitationId, opt => opt.MapFrom(x => x.InvitationId))
            .ForMember(x => x.UserSenderId, opt => opt.MapFrom(x => x.UserFeedbackSenderId))
            .ForMember(x => x.UserSenderFullName, opt => opt.MapFrom(x =>
                x.UserFeedbackSender.FullName))
            .ForMember(x => x.UserSenderImageUrl, opt => opt.MapFrom(x =>
                x.UserFeedbackSender.ImageUrl))
           .ForMember(x => x.UserSenderFullLocation, opt => opt.MapFrom(x =>
                x.UserFeedbackSender.FullLocation))
            .ForMember(x => x.WasTheJointLesson, opt => opt.MapFrom(x => x.WasTheJointLesson))
            .ForMember(x => x.FeedbackText, opt => opt.MapFrom(x => x.FeedbackText))
            .ForMember(x => x.Rating, opt => opt.MapFrom(x => x.Rating))
            .ForMember(x => x.CreatedAt, opt => opt.MapFrom(x => x.CreatedAt));
    }
}