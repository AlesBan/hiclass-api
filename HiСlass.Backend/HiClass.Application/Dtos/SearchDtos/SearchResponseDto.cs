using HiClass.Application.Dtos.ClassDtos;
using HiClass.Application.Dtos.UserDtos;

namespace HiClass.Application.Dtos.SearchDtos;

public class SearchResponseDto
{
    public IEnumerable<UserProfileDto> TeacherProfiles { get; init; } = new List<UserProfileDto>();
    public IEnumerable<UserProfileDto> ExpertProfiles { get; init; } = new List<UserProfileDto>();
    public IEnumerable<ClassProfileDto> ClassProfiles { get; init; } = new List<ClassProfileDto>();
}