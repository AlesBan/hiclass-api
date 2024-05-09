using AutoMapper;
using HiClass.Application.Common.Mappings;
using HiClass.Application.Models.Class;
using HiClass.Application.Models.Institution;
using HiClass.Application.Models.Invitations.Feedbacks;
using HiClass.Domain.Entities.Communication;
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
    public IEnumerable<ClassProfileDto> ClassDtos { get; set; } = new List<ClassProfileDto>();
    public List<string> LanguageTitles { get; set; } = new();
    public List<string> DisciplineTitles { get; set; } = new();
    public List<int> GradeNumbers { get; set; } = new();
    public IEnumerable<FeedbackDto> FeedbackDtos { get; set; } = new List<FeedbackDto>();

    public void Mapping(Profile profile)
    {
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
                ));
    }
}