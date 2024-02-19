using AutoMapper;

namespace HiClass.Application.Models.Institution;

public class InstitutionDto
{

    public List<string> Types { get; set; }
    public string Address { get; set; }
    public string Title { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Entities.Job.Institution, InstitutionDto>()
            .ForMember(dest => dest.Address,
                opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Title,
                opt => opt.MapFrom(src => src.Title));
    }
}