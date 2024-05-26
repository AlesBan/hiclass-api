using AutoMapper;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetUserProfileListByDefaultSearchRequest;
using HiClass.Application.Helpers.UserHelper;
using HiClass.Application.Models.Class;
using HiClass.Application.Models.Search;
using HiClass.Application.Models.User;
using HiClass.Domain.Entities.Main;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HiClass.Infrastructure.Services.DefaultDataServices;

public class DefaultSearchService : IDefaultSearchService
{
    private readonly IUserHelper _userHelper;
    private readonly IMapper _mapper;
    private readonly ILogger<DefaultSearchService> _logger;

    public DefaultSearchService(IUserHelper userHelper, IMapper mapper, ILogger<DefaultSearchService> logger)
    {
        _userHelper = userHelper;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<DefaultSearchResponseDto> GetDefaultTeacherAndClassProfiles(Guid userId, IMediator mediator)
    {
        var user = await _userHelper.GetUserById(userId, mediator);
        var searchRequest = await CreateDefaultSearchRequestDto(user);
        var userList = await GetUserListByDefaultSearchResponse(searchRequest, mediator);
        var defaultSearchResponseDto = GetDefaultSearchResponseDto(user, userList);
        return defaultSearchResponseDto;
    }

    private static Task<DefaultSearchCommandDto> CreateDefaultSearchRequestDto(User user)
    {
        var userDisciplineIds = user.UserDisciplines.Select(ud => ud.DisciplineId).ToList();
        var userCountryId = user.CountryId ?? Guid.Empty;

        return Task.FromResult(new DefaultSearchCommandDto
        {
            UserId = user.UserId,
            DisciplineIds = userDisciplineIds,
            CountryId = userCountryId
        });
    }

    private static async Task<IEnumerable<User>> GetUserListByDefaultSearchResponse(
        DefaultSearchCommandDto searchRequest,
        IMediator mediator)
    {
        var query = new GetUserListByDefaultSearchRequestCommand
        {
            SearchRequest = searchRequest
        };

        var userList = (await mediator.Send(query)).ToList();

        return userList;
    }

    private DefaultSearchResponseDto GetDefaultSearchResponseDto(User user, IEnumerable<User> userList)
    {
        var userProfileList = GetUserProfileList(userList);
        var userDisciplineTitles = user.UserDisciplines
            .Select(ud => ud.Discipline.Title).ToList();

        var userProfileDtos = userProfileList.ToList();

        var countryTitle = user.Country?.Title ?? string.Empty;
        if (user.Country == null)
        {
            _logger.LogWarning("User country is null for user ID {UserId}", user.UserId);
        }

        var teacherProfilesByCountry = GetProfilesByCountry(countryTitle, userProfileDtos, isTeacher: true);
        var expertProfilesByCountry = GetProfilesByCountry(countryTitle, userProfileDtos, isTeacher: false);
        var teacherProfilesByDisciplines =
            GetProfilesByDisciplines(user.UserDisciplines.Select(ud => ud.Discipline.Title), userProfileDtos,
                isTeacher: true);
        var expertProfilesByDisciplines =
            GetProfilesByDisciplines(user.UserDisciplines.Select(ud => ud.Discipline.Title), userProfileDtos,
                isTeacher: false);

        var profilesByCountry = teacherProfilesByCountry.ToList();
        var classProfilesByCountry = GetClassProfiles(profilesByCountry).ToList();

        var profilesByDisciplines = teacherProfilesByDisciplines.ToList();
        var classProfilesByDisciplines = GetClassProfiles(profilesByDisciplines, userDisciplineTitles).ToList();

        return new DefaultSearchResponseDto
        {
            TeacherProfilesByCountry = profilesByCountry,
            ExpertProfilesByCountry = expertProfilesByCountry,
            TeacherProfilesByDisciplines = profilesByDisciplines,
            ExpertProfilesByDisciplines = expertProfilesByDisciplines,
            ClassProfilesByCountry = classProfilesByCountry,
            ClassProfilesByDisciplines = classProfilesByDisciplines
        };
    }

    private IEnumerable<UserProfileDto> GetUserProfileList(IEnumerable<User> userList)
    {
        var userProfileDtos = userList
            .Select(user => _mapper.Map<UserProfileDto>(user));
        return userProfileDtos;
    }

    private static IEnumerable<UserProfileDto> GetProfilesByCountry(string userCountry,
        IEnumerable<UserProfileDto> userProfileList, bool isTeacher)
    {
        return userProfileList
            .Where(up => up.CountryTitle == userCountry && up.IsATeacher == isTeacher);
    }

    private static IEnumerable<UserProfileDto> GetProfilesByDisciplines(IEnumerable<string> userDisciplines,
        IEnumerable<UserProfileDto> userProfileList, bool isTeacher)
    {
        return userProfileList
            .Where(up => up.DisciplineTitles
                .Intersect(userDisciplines)
                .Any() && up.IsATeacher == isTeacher);
    }

    private static IEnumerable<ClassProfileDto> GetClassProfiles(IEnumerable<UserProfileDto> userProfileList)
    {
        return userProfileList
            .Select(c => c.ClassDtos)
            .SelectMany(cl => cl)
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

    private static IEnumerable<ClassProfileDto> GetClassProfiles(IEnumerable<UserProfileDto> userProfileList,
        IEnumerable<string> disciplineIds)
    {
        return userProfileList
            .Select(c => c.ClassDtos)
            .SelectMany(cl => cl)
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
            })
            .Where(c =>
                c.Disciplines.Any(disciplineIds.Contains));
    }
}