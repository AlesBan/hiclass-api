using AutoMapper;

namespace HiClass.Application.Models.Institution;

public class InstitutionDto
{
    public List<string> Types { get; set; } = new List<string>();
    public string Address { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Entities.Job.Institution, InstitutionDto>()
            .ForMember(dest => dest.Address,
                opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Title,
                opt => opt.MapFrom(src => src.Title));
    }
}