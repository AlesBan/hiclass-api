using HiClass.Application.Models.Class;
using HiClass.Application.Models.User;

namespace HiClass.Application.Models.Search;

public class DefaultSearchResponseDto
{
    public IEnumerable<UserProfileDto> TeacherProfilesByCountry { get; init; } = new List<UserProfileDto>();
    public IEnumerable<UserProfileDto> ExpertProfilesByCountry { get; init; } = new List<UserProfileDto>();
    public IEnumerable<UserProfileDto> TeacherProfilesByDisciplines { get; init; } = new List<UserProfileDto>();
    public IEnumerable<UserProfileDto> ExpertProfilesByDisciplines { get; init; } = new List<UserProfileDto>();
    public IEnumerable<ClassProfileDto> ClassProfilesByCountry { get; init; } = new List<ClassProfileDto>();
    public IEnumerable<ClassProfileDto> ClassProfilesByDisciplines { get; init; } = new List<ClassProfileDto>();
}