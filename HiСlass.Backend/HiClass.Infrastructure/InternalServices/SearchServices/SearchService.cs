using AutoMapper;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetUserProfileListBySearchRequest;
using HiClass.Application.Models.Class;
using HiClass.Application.Models.Search;
using HiClass.Application.Models.User;
using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Infrastructure.InternalServices.SearchServices;

public class SearchService : ISearchService
{
    private readonly IMapper _mapper;

    public SearchService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<SearchResponseDto> GetTeacherAndClassProfiles(Guid userId, SearchRequestDto searchRequest, IMediator mediator)
    {
        var userList = await GetUserListBySearchRequest(userId, searchRequest, mediator);
        var searchResponseDto = await GetSearchResponseDto(userList, searchRequest.Disciplines, searchRequest.Languages,
            searchRequest.Grades);
        return searchResponseDto;
    }

    private static async Task<IEnumerable<User>> GetUserListBySearchRequest(Guid userId, SearchRequestDto searchRequest, IMediator mediator)
    {
        var query = new GetUserListBySearchRequestCommand
        {
            SearchRequest = new SearchCommandDto
            {
                UserId = userId,
                Disciplines = searchRequest.Disciplines,
                Languages = searchRequest.Languages,
                Grades = searchRequest.Grades,
                Countries = searchRequest.Countries
            }
        };

        var userList = (await mediator.Send(query)).ToList();

        return userList;
    }

    private async Task<SearchResponseDto> GetSearchResponseDto(IEnumerable<User> userList, IEnumerable<string> disciplines, IEnumerable<string> languages, IEnumerable<int> grades)
    {
        var userProfileList = (await GetUserProfileList(userList)).ToList();
        var teacherProfiles = GetProfiles(userProfileList, isTeacher: true).ToList();
        var expertProfiles = GetProfiles(userProfileList, isTeacher: false).ToList();
        var classProfiles = GetClassProfiles(teacherProfiles, disciplines.ToList(), languages.ToList(), grades.ToList()).ToList();

        return new SearchResponseDto
        {
            TeacherProfiles = teacherProfiles,
            ExpertProfiles = expertProfiles,
            ClassProfiles = classProfiles
        };
    }

    private async Task<IEnumerable<UserProfileDto>> GetUserProfileList(IEnumerable<User> userList)
    {
        return userList.Select(user => _mapper.Map<UserProfileDto>(user));
    }

    private static IEnumerable<UserProfileDto> GetProfiles(IEnumerable<UserProfileDto> userProfileList, bool isTeacher)
    {
        return userProfileList.Where(up => up.IsATeacher == isTeacher);
    }

    private static IEnumerable<ClassProfileDto> GetClassProfiles(IEnumerable<UserProfileDto> teacherProfileDtos, List<string> disciplines, List<string> languages, List<int> grades)
    {
        var classProfiles = teacherProfileDtos
            .SelectMany(tp => tp.ClassDtos)
            .Where(c => 
                (!disciplines.Any() || c.Disciplines.Intersect(disciplines, StringComparer.OrdinalIgnoreCase).Any()) &&
                (!languages.Any() || c.Languages.Intersect(languages, StringComparer.OrdinalIgnoreCase).Any()) &&
                (!grades.Any() || grades.Contains(c.Grade)))
            .Select(c => new ClassProfileDto
            {
                ClassId = c.ClassId,
                Title = c.Title,
                UserFullName = c.UserFullName,
                UserRating = c.UserRating,
                Grade = c.Grade,
                Languages = c.Languages,
                Disciplines = c.Disciplines,
                ImageUrl = c.ImageUrl
            });

        return classProfiles;
    }
}
