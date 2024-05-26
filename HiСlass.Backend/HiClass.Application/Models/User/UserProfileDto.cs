using AutoMapper;
using HiClass.Application.Common.Mappings;
using HiClass.Application.Models.Class;
using HiClass.Application.Models.Institution;
using HiClass.Application.Models.Invitations.Feedbacks;
using HiClass.Domain.Entities.Communication;
using HiClass.Domain.Entities.Education;
using HiClass.Domain.Entities.Location;

namespace HiClass.Application.Models.User;

public class UserProfileDto : IMapWith<Domain.Entities.Main.User>
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string BannerImageUrl { get; set; } = string.Empty;
    public bool IsATeacher { get; set; }
    public bool IsAnExpert { get; set; }
    public string CityTitle { get; set; } = string.Empty;
    public string CountryTitle { get; set; } = string.Empty;
    public InstitutionDto Institution { get; set; }
    public double Rating { get; set; }
    public List<string> LanguageTitles { get; set; } = new();
    public List<string> DisciplineTitles { get; set; } = new();
    public List<int> GradeNumbers { get; set; } = new();
    public IEnumerable<ClassProfileDto> ClassDtos { get; set; } = new List<ClassProfileDto>();
    public IEnumerable<FeedbackDto> FeedbackDtos { get; set; } = new List<FeedbackDto>();

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
            .ForMember(x => x.UserSenderFullLocation, opt => opt.MapFrom(x =>
                x.UserSender.FullLocation))
            .ForMember(x => x.WasTheJointLesson, opt => opt.MapFrom(x => x.WasTheJointLesson))
            .ForMember(x => x.FeedbackText, opt => opt.MapFrom(x => x.FeedbackText))
            .ForMember(x => x.Rating, opt => opt.MapFrom(x => x.Rating))
            .ForMember(x => x.CreatedAt, opt => opt.MapFrom(x => x.CreatedAt));
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
            .ForMember(up => up.Languages,
                opt => opt.MapFrom(u => u.ClassLanguages.Select(cl =>
                    cl.Language.Title)))
            .ForMember(up => up.Disciplines,
                opt => opt.MapFrom(u => u.ClassDisciplines.Select(cd =>
                    cd.Discipline.Title)));
        profile.CreateMap<City, UserProfileDto>()
            .ForMember(up => up.CityTitle,
                opt => opt.MapFrom(c => c.Title));
        profile.CreateMap<Country, UserProfileDto>()
            .ForMember(up => up.CountryTitle,
                opt => opt.MapFrom(c => c.Title));
        profile.CreateMap<Domain.Entities.Job.Institution, InstitutionDto>()
            .ForMember(dest => dest.Address,
                opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Title,
                opt => opt.MapFrom(src => src.Title));
        profile.CreateMap<Domain.Entities.Main.User, UserProfileDto>()
            .ForMember(up => up.UserId,
                opt => opt.MapFrom(u => u.UserId))
            .ForMember(up => up.Email,
                opt => opt.MapFrom(u => u.Email))
            .ForMember(up => up.FirstName,
                opt => opt.MapFrom(u => u.FirstName))
            .ForMember(up => up.LastName,
                opt => opt.MapFrom(u => u.LastName))
            .ForMember(up => up.Description,
                opt => opt.MapFrom(u => u.Description))
            .ForMember(up => up.ImageUrl,
                opt => opt.MapFrom(u => u.ImageUrl))
            .ForMember(up => up.BannerImageUrl,
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
            .ForMember(up => up.LanguageTitles,
                opt => opt.MapFrom(u => u.UserLanguages.Select(cl =>
                    cl.Language.Title)))
            .ForMember(up => up.DisciplineTitles,
                opt => opt.MapFrom(u => u.UserDisciplines.Select(cd =>
                    cd.Discipline.Title)))
            .ForMember(up => up.GradeNumbers,
                opt => opt.MapFrom(u => u.UserGrades.Select(cd =>
                    cd.Grade.GradeNumber)))
            .ForMember(dest => dest.ClassDtos,
                opt => opt.MapFrom(src => src.Classes))
            .ForMember(dest => dest.FeedbackDtos,
                opt => opt.MapFrom(src => src.ReceivedFeedbacks));
    }
}