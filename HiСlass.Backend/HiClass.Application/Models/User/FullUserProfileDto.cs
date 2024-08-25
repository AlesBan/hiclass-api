using AutoMapper;
using HiClass.Application.Common.Mappings;
using HiClass.Application.Models.Class;
using HiClass.Application.Models.Device;
using HiClass.Application.Models.Institution;
using HiClass.Application.Models.Invitations.Feedbacks;
using HiClass.Application.Models.Invitations.Invitations;
using HiClass.Domain.Entities.Communication;
using HiClass.Domain.Entities.Education;
using HiClass.Domain.Entities.Location;

namespace HiClass.Application.Models.User;

public class FullUserProfileDto : IMapWith<Domain.Entities.Main.User>
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public bool IsVerified { get; set; }
    public string VerificationCode { get; set; } = string.Empty;
    public string PasswordResetCode { get; set; } = string.Empty;
    public bool IsCreateAccount { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string BannerPhotoUrl { get; set; } = string.Empty;
    public bool IsATeacher { get; set; }
    public bool IsAnExpert { get; set; }
    public string CityTitle { get; set; } = string.Empty;
    public string CountryTitle { get; set; } = string.Empty;
    public InstitutionDto Institution { get; set; }
    public double Rating { get; set; }
    public IEnumerable<ClassProfileDto> ClassDtos { get; set; } = new List<ClassProfileDto>();
    public List<string> LanguageTitles { get; set; } = new();
    public List<string> DisciplineTitles { get; set; } = new();
    public List<int> GradeNumbers { get; set; } = new();
    public IEnumerable<FeedbackDto> FeedbackDtos { get; set; } = new List<FeedbackDto>();
    public IEnumerable<InvitationDto> AllInvitationDtos { get; set; } = new List<InvitationDto>();
    public IEnumerable<DeviceDto> DeviceDtos { get; set; } = new List<DeviceDto>();

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Entities.Notifications.Device, DeviceDto>()
            .ForMember(x => x.DeviceToken, opt => opt.MapFrom(x => x.DeviceToken))
            .ForMember(x => x.DeviceId, opt => opt.MapFrom(x => x.DeviceId));

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
        profile.CreateMap<Invitation, InvitationDto>()
            .ForMember(x => x.Feedbacks, opt => opt.MapFrom(x => x.Feedbacks))
            .ForMember(x => x.Status, opt => opt.MapFrom(x => x.Status))
            .ForMember(x => x.InvitationText, opt => opt.MapFrom(x => x.InvitationText))
            .ForMember(x => x.CreatedAt, opt => opt.MapFrom(x => x.CreatedAt))
            .ForMember(x => x.DateOfInvitation, opt => opt.MapFrom(x => x.DateOfInvitation))
            .ForMember(x => x.UserSenderId, opt => opt.MapFrom(x => x.UserSenderId))
            .ForMember(x => x.UserReceiverId, opt => opt.MapFrom(x => x.UserRecipientId))
            .ForMember(x => x.ClassSenderId, opt => opt.MapFrom(x => x.ClassSenderId))
            .ForMember(x => x.ClassReceiverId, opt => opt.MapFrom(x => x.ClassRecipientId));
        profile.CreateMap<Domain.Entities.Main.Class, Grade>()
            .ForMember(up => up.GradeNumber,
                opt => opt.MapFrom(u => u.Grade));
        profile.CreateMap<Domain.Entities.Main.Class, ClassProfileDto>()
            .ForMember(cp => cp.ClassId,
                opt => opt.MapFrom(u => u.ClassId))
            .ForMember(up => up.Title,
                opt => opt.MapFrom(u => u.Title))
            .ForMember(up => up.UserFullName,
                opt => opt.MapFrom(u =>
                    u.User.FirstName + " " + u.User.LastName))
            .ForMember(up => up.UserRating,
                opt => opt.MapFrom(u => u.User.Rating))
            .ForMember(up => up.UserFeedbacksCount,
                opt => opt.MapFrom(u => u.User.ReceivedFeedbacks.Count))
            .ForMember(up => up.Grade,
                opt => opt.MapFrom(u => u.Grade.GradeNumber))
            .ForMember(up => up.ImageUrl,
                opt => opt.MapFrom(u => u.ImageUrl))
            .ForMember(up => up.LanguageTitles,
                opt => opt.MapFrom(u => u.ClassLanguages.Select(cl =>
                    cl.Language.Title)))
            .ForMember(up => up.DisciplineTitle,
                opt => opt.MapFrom(u => u.Discipline.Title));
        profile.CreateMap<City, FullUserProfileDto>()
            .ForMember(up => up.CityTitle,
                opt => opt.MapFrom(c => c.Title));
        profile.CreateMap<Country, FullUserProfileDto>()
            .ForMember(up => up.CountryTitle,
                opt => opt.MapFrom(c => c.Title));
        profile.CreateMap<Domain.Entities.Job.Institution, InstitutionDto>()
            .ForMember(dest => dest.Address,
                opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Title,
                opt => opt.MapFrom(src => src.Title));
        profile.CreateMap<Domain.Entities.Main.User, FullUserProfileDto>()
            .ForMember(up => up.UserId,
                opt => opt.MapFrom(u => u.UserId))
            .ForMember(up => up.Email,
                opt => opt.MapFrom(u => u.Email))
            .ForMember(up => up.IsVerified,
                opt => opt.MapFrom(u => u.IsVerified))
            .ForMember(up => up.VerificationCode,
                opt => opt.MapFrom(u => u.VerificationCode))
            .ForMember(up => up.PasswordResetCode,
                opt => opt.MapFrom(u => u.PasswordResetCode))
            .ForMember(up => up.IsCreateAccount,
                opt => opt.MapFrom(u => u.IsCreatedAccount))
            .ForMember(up => up.FirstName,
                opt => opt.MapFrom(u => u.FirstName))
            .ForMember(up => up.LastName,
                opt => opt.MapFrom(u => u.LastName))
            .ForMember(up => up.Description,
                opt => opt.MapFrom(u => u.Description))
            .ForMember(up => up.ImageUrl,
                opt => opt.MapFrom(u => u.ImageUrl))
            .ForMember(up => up.BannerPhotoUrl,
                opt => opt.MapFrom(u => u.BannerImageUrl))
            .ForMember(up => up.IsATeacher,
                opt => opt.MapFrom(u => u.IsATeacher))
            .ForMember(up => up.IsAnExpert,
                opt => opt.MapFrom(u => u.IsAnExpert))
            .ForMember(up => up.Rating,
                opt => opt.MapFrom(u => u.Rating))
            .ForMember(up => up.CityTitle,
                opt => opt.MapFrom(c => c.City.Title))
            .ForMember(up => up.CountryTitle,
                opt => opt.MapFrom(c => c.Country.Title))
            .ForMember(dest => dest.Institution,
                opt => opt.MapFrom(src => new InstitutionDto
                    {
                        Address = src.Institution.Address,
                        Title = src.Institution.Title
                    }
                ))
            .ForMember(dest => dest.LanguageTitles,
                opt => opt.MapFrom(src => src.UserLanguages.Select(cl =>
                    cl.Language.Title)))
            .ForMember(dest => dest.DisciplineTitles,
                opt => opt.MapFrom(src => src.UserDisciplines.Select(cd =>
                    cd.Discipline.Title)))
            .ForMember(dest => dest.GradeNumbers,
                opt => opt.MapFrom(src => src.UserGrades.Select(cd =>
                    cd.Grade.GradeNumber)))
            .ForMember(dest => dest.ClassDtos,
                opt => opt.MapFrom(src => src.Classes))
            .ForMember(dest => dest.AllInvitationDtos,
                opt => opt.MapFrom(src => src.ReceivedInvitations.Concat(src.SentInvitations)))
            .ForMember(dest => dest.FeedbackDtos,
                opt => opt.MapFrom(src => src.ReceivedFeedbacks))
            .ForMember(dest => dest.DeviceDtos, opt =>
                opt.MapFrom(src => src.UserDevices.Select(d => d.Device)));
    }
}