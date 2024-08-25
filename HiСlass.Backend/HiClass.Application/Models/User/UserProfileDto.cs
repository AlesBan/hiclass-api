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
    public bool IsVerified { get; set; } 
    public bool IsCreatedAccount { get; set; } 
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
            .ForMember(f => f.FeedbackId, opt => opt.MapFrom(f => f.FeedbackId))
            .ForMember(f => f.InvitationId, opt => opt.MapFrom(f => f.InvitationId))
            .ForMember(f => f.UserSenderId, opt => opt.MapFrom(f => f.UserFeedbackSenderId))
            .ForMember(f => f.UserSenderFullName, opt => opt.MapFrom(f => f.UserFeedbackSender.FullName))
            .ForMember(f => f.UserSenderImageUrl, opt => opt.MapFrom(f => f.UserFeedbackSender.ImageUrl))
            .ForMember(f => f.UserSenderFullLocation, opt => opt.MapFrom(f => f.UserFeedbackSender.FullLocation))
            .ForMember(f => f.WasTheJointLesson, opt => opt.MapFrom(f => f.WasTheJointLesson))
            .ForMember(f => f.FeedbackText, opt => opt.MapFrom(f => f.FeedbackText))
            .ForMember(f => f.Rating, opt => opt.MapFrom(f => f.Rating))
            .ForMember(f => f.CreatedAt, opt => opt.MapFrom(f => f.CreatedAt));

        profile.CreateMap<Domain.Entities.Main.Class, Grade>()
            .ForMember(g => g.GradeNumber, opt => opt.MapFrom(c => c.Grade));

        profile.CreateMap<Domain.Entities.Main.Class, ClassProfileDto>()
            .ForMember(cp => cp.ClassId, opt => opt.MapFrom(c => c.ClassId))
            .ForMember(cp => cp.Title, opt => opt.MapFrom(c => c.Title))
            .ForMember(cp => cp.UserFullName, opt => opt.MapFrom(c => c.User.FirstName + " " + c.User.LastName))
            .ForMember(cp => cp.UserRating, opt => opt.MapFrom(c => c.User.Rating))
            .ForMember(cp => cp.UserFeedbacksCount, opt => opt.MapFrom(c => c.User.ReceivedFeedbacks.Count))
            .ForMember(cp => cp.Grade, opt => opt.MapFrom(c => c.Grade.GradeNumber))
            .ForMember(cp => cp.ImageUrl, opt => opt.MapFrom(c => c.ImageUrl))
            .ForMember(cp => cp.LanguageTitles, opt => opt.MapFrom(c => c.ClassLanguages.Select(cl => cl.Language.Title)))
            .ForMember(cp => cp.DisciplineTitle, opt => opt.MapFrom(c => c.Discipline.Title));

        profile.CreateMap<City, UserProfileDto>()
            .ForMember(u => u.CityTitle, opt => opt.MapFrom(c => c.Title));

        profile.CreateMap<Country, UserProfileDto>()
            .ForMember(u => u.CountryTitle, opt => opt.MapFrom(c => c.Title));

        profile.CreateMap<Domain.Entities.Job.Institution, InstitutionDto>()
            .ForMember(i => i.Address, opt => opt.MapFrom(i => i.Address))
            .ForMember(i => i.Title, opt => opt.MapFrom(i => i.Title));

        profile.CreateMap<Domain.Entities.Main.User, UserProfileDto>()
            .ForMember(u => u.UserId, opt => opt.MapFrom(u => u.UserId))
            .ForMember(u => u.Email, opt => opt.MapFrom(u => u.Email))
            .ForMember(u => u.IsVerified, opt => opt.MapFrom(u => u.IsVerified))
            .ForMember(u => u.IsCreatedAccount, opt => opt.MapFrom(u => u.IsCreatedAccount))
            .ForMember(u => u.FirstName, opt => opt.MapFrom(u => u.FirstName))
            .ForMember(u => u.LastName, opt => opt.MapFrom(u => u.LastName))
            .ForMember(u => u.Description, opt => opt.MapFrom(u => u.Description))
            .ForMember(u => u.ImageUrl, opt => opt.MapFrom(u => u.ImageUrl))
            .ForMember(u => u.BannerImageUrl, opt => opt.MapFrom(u => u.BannerImageUrl))
            .ForMember(u => u.IsATeacher, opt => opt.MapFrom(u => u.IsATeacher))
            .ForMember(u => u.IsAnExpert, opt => opt.MapFrom(u => u.IsAnExpert))
            .ForMember(u => u.Rating, opt => opt.MapFrom(u => u.Rating))
            .ForMember(u => u.CityTitle, opt => opt.MapFrom(u => u.City.Title))
            .ForMember(u => u.CountryTitle, opt => opt.MapFrom(u => u.Country.Title))
            .ForMember(u => u.Institution, opt => opt.MapFrom(u => new InstitutionDto
            {
                Address = u.Institution.Address,
                Title = u.Institution.Title
            }))
            .ForMember(u => u.LanguageTitles, opt => opt.MapFrom(u => u.UserLanguages.Select(l => l.Language.Title)))
            .ForMember(u => u.DisciplineTitles, opt => opt.MapFrom(u => u.UserDisciplines.Select(d => d.Discipline.Title)))
            .ForMember(u => u.GradeNumbers, opt => opt.MapFrom(u => u.UserGrades.Select(g => g.Grade.GradeNumber)))
            .ForMember(u => u.ClassDtos, opt => opt.MapFrom(u => u.Classes))
            .ForMember(u => u.FeedbackDtos, opt => opt.MapFrom(u => u.ReceivedFeedbacks));
    }
}
