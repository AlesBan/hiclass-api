using HiClass.Application.Dtos.UserDtos;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetUserProfileListBySearchRequest;
using HiClass.Application.Helpers.UserHelper;
using HiClass.Application.Interfaces.Services;
using HiClass.Application.Models.Class;
using HiClass.Application.Models.Search;
using HiClass.Application.Models.User;
using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Infrastructure.Services.SearchService;

public class SearchService : ISearchService
{
    private readonly IUserHelper _userHelper;

    public SearchService(IUserHelper userHelper)
    {
        _userHelper = userHelper;
    }

    public async Task<SearchResponseDto> GetTeacherAndClassProfiles(Guid userId, SearchRequestDto searchRequest,
        IMediator mediator)
    {
        var userList = await GetUserListBySearchRequest(userId, searchRequest, mediator);
        var searchResponseDto = await GetSearchResponseDto(userList, searchRequest.Disciplines);
        return searchResponseDto;
    }

    private static async Task<IEnumerable<User>> GetUserListBySearchRequest(Guid userId, SearchRequestDto searchRequest,
        IMediator mediator)
    {
        var query = new GetUserListBySearchRequestCommand
        {
            SearchRequest = new SearchCommandDto()
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

    private async Task<SearchResponseDto> GetSearchResponseDto(IEnumerable<User> userList,
        IEnumerable<string> languages)
    {
        var userProfileList = (await GetUserProfileList(userList)).ToList();
        var teacherProfiles = GetProfiles(userProfileList, isTeacher: true).ToList();
        var expertProfiles = GetProfiles(userProfileList, isTeacher: false).ToList();
        var classProfiles = GetClassProfiles(teacherProfiles, languages).ToList();

        return new SearchResponseDto
        {
            TeacherProfiles = teacherProfiles,
            ExpertProfiles = expertProfiles,
            ClassProfiles = classProfiles
        };
    }

    private async Task<IEnumerable<UserProfileDto>> GetUserProfileList(IEnumerable<User> userList)
    {
        var userProfileDtos = await Task.WhenAll(userList
            .Select(u =>
                _userHelper.MapUserToUserProfileDto(u)));
        return userProfileDtos;
    }

    private static IEnumerable<UserProfileDto> GetProfiles(IEnumerable<UserProfileDto> userProfileList, bool isTeacher)
    {
        return userProfileList
            .Where(up => up.IsATeacher == isTeacher);
    }

    private static IEnumerable<ClassProfileDto> GetClassProfiles(IEnumerable<UserProfileDto> teacherProfiles,
        IEnumerable<string> disciplines)
    {
        return teacherProfiles
            .SelectMany(tp => tp.ClassDtos)
            .Where(c => c.Disciplines.Intersect(disciplines,
                StringComparer.OrdinalIgnoreCase).Any())
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
    }
}