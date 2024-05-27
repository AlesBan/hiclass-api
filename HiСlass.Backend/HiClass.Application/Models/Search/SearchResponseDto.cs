using HiClass.Application.Models.Class;
using HiClass.Application.Models.User;

namespace HiClass.Application.Models.Search;

public class SearchResponseDto
{
    public IEnumerable<UserProfileDto> TeacherProfiles { get; init; } = new List<UserProfileDto>();
    public IEnumerable<UserProfileDto> ExpertProfiles { get; init; } = new List<UserProfileDto>();
    public IEnumerable<ClassProfileDto> ClassProfiles { get; init; } = new List<ClassProfileDto>();
}