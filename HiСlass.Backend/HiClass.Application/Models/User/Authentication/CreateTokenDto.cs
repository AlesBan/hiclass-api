using System.ComponentModel.DataAnnotations;
using AutoMapper;
using HiClass.Application.Common.Mappings;

namespace HiClass.Application.Models.User.Authentication;

public class CreateTokenDto : IMapWith<Domain.Entities.Main.User>
{
    [Required] public Guid UserId { get; set; }
    [Required] public string Email { get; set; } = null!;
    [Required] public bool IsVerified { get; set; }
    [Required] public bool IsCreatedAccount { get; set; }
    [Required] public bool IsATeacher { get; set; }
    [Required] public bool IsAnExpert { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Entities.Main.User, CreateTokenDto>()
            .ForMember(dest => dest.UserId,
                opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Email,
                opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.IsVerified,
                opt => opt.MapFrom(src => src.IsVerified))
            .ForMember(dest => dest.IsCreatedAccount,
                opt => opt.MapFrom(src => src.IsCreatedAccount))
            .ForMember(dest => dest.IsATeacher,
                opt => opt.MapFrom(src => src.IsATeacher))
            .ForMember(dest => dest.IsAnExpert,
                opt => opt.MapFrom(src => src.IsAnExpert));
    }
}