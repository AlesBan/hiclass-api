using AutoMapper;
using HiClass.Application.Common.Mappings;
using HiClass.Domain.Entities.Education;

namespace HiClass.Application.Models.Class;

public class ClassProfileDto : IMapWith<Domain.Entities.Main.Class>
{
    public Guid ClassId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string UserFullName { get; set; } = string.Empty;
    public double UserRating { get; set; }
    public int UserFeedbacksCount { get; set; }
    public int Grade { get; set; }
    public List<string> Languages { get; set; } = new();
    public List<string> Disciplines { get; set; } = new();
    public string PhotoUrl { get; set; } = string.Empty;

    public void Mapping(Profile profile)
    {
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
            .ForMember(up => up.PhotoUrl,
                opt => opt.MapFrom(u => u.PhotoUrl))
            .ForMember(up => up.Languages,
                opt => opt.MapFrom(u => u.ClassLanguages.Select(cl =>
                    cl.Language.Title)))
            .ForMember(up => up.Disciplines,
                opt => opt.MapFrom(u => u.ClassDisciplines.Select(cd =>
                    cd.Discipline.Title)));
    }
}

class ClassProfileDtoImpl : ClassProfileDto
{
}