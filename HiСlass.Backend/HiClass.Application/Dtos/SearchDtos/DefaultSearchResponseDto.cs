using HiClass.Application.Dtos.ClassDtos;
using HiClass.Application.Dtos.UserDtos;

namespace HiClass.Application.Dtos.SearchDtos;

public class DefaultSearchResponseDto
{
    public IEnumerable<UserProfileDto> TeacherProfilesByCountry { get; init; } = new List<UserProfileDto>();
    public IEnumerable<UserProfileDto> ExpertProfilesByCountry { get; init; } = new List<UserProfileDto>();
    public IEnumerable<UserProfileDto> TeacherProfilesByDisciplines { get; init; } = new List<UserProfileDto>();
    public IEnumerable<UserProfileDto> ExpertProfilesByDisciplines { get; init; } = new List<UserProfileDto>();
    public IEnumerable<ClassProfileDto> ClassProfilesByCountry { get; init; } = new List<ClassProfileDto>();
    public IEnumerable<ClassProfileDto> ClassProfilesByDisciplines { get; init; } = new List<ClassProfileDto>();
}