using HiClass.Application.Dtos.UserDtos;
using HiClass.Application.Models.Class;

namespace HiClass.Application.Models.Search;

public class SearchResponseDto
{
    public IEnumerable<UserProfileDto> TeacherProfiles { get; init; } = new List<UserProfileDto>();
    public IEnumerable<UserProfileDto> ExpertProfiles { get; init; } = new List<UserProfileDto>();
    public IEnumerable<ClassProfileDto> ClassProfiles { get; init; } = new List<ClassProfileDto>();
}