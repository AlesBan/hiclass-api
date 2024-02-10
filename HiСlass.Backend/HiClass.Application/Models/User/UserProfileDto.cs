using AutoMapper;
using HiClass.Application.Common.Mappings;
using HiClass.Application.Models.Class;
using HiClass.Application.Models.Institution;
using HiClass.Domain.Entities.Communication;
using HiClass.Domain.Entities.Job;
using HiClass.Domain.Entities.Location;
using HiClass.Domain.Entities.Main;

namespace HiClass.Application.Dtos.UserDtos;

public class UserProfileDto : IMapWith<User>
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public bool IsVerified { get; set; }
    public string VerificationCode { get; set; }
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
    public IEnumerable<ClassProfileDto> ClasseDtos { get; set; } = new List<ClassProfileDto>();
    public List<string> LanguageTitles { get; set; } = new();
    public List<string> DisciplineTitles { get; set; } = new();
    public List<int> GradeNumbers { get; set; } = new();
    public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public void Mapping(Profile profile)
    {
        profile.CreateMap<City, UserProfileDto>()
            .ForMember(up => up.CityTitle,
                opt => opt.MapFrom(c => c.Title));
        profile.CreateMap<Country, UserProfileDto>()
            .ForMember(up => up.CountryTitle,
                opt => opt.MapFrom(c => c.Title));
        profile.CreateMap<Institution, InstitutionDto>()
            .ForMember(dest => dest.Address,
                opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Title,
                opt => opt.MapFrom(src => src.Title));
        profile.CreateMap<User, UserProfileDto>()
            .ForMember(up => up.UserId,
                opt => opt.MapFrom(u => u.UserId))
            .ForMember(up => up.Email,
                opt => opt.MapFrom(u => u.Email))
            .ForMember(up => up.AccessToken,
                opt => opt.MapFrom(u => u.AccessToken))
            .ForMember(up => up.IsVerified,
                opt => opt.MapFrom(u => u.IsVerified))
            .ForMember(up => up.VerificationCode,
                opt => opt.MapFrom(u => u.VerificationCode))
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
                opt => opt.MapFrom(u => u.BannerPhotoUrl))
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
                    Title = src.Institution.Title,
                }));
    }
}