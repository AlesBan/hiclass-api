using AutoMapper;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetUserProfileListByDefaultSearchRequest;
using HiClass.Application.Helpers.UserHelper;
using HiClass.Application.Models.Class;
using HiClass.Application.Models.Search;
using HiClass.Application.Models.User;
using HiClass.Domain.Entities.Main;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HiClass.Infrastructure.InternalServices.SearchServices;

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
        var user = await _userHelper.GetFullUserById(userId, mediator);
        var userList = await GetUserListByDefaultSearchResponse(user, mediator);
        return CreateDefaultSearchResponseDto(user, userList);
    }

    private static async Task<IEnumerable<User>> GetUserListByDefaultSearchResponse(
        User user, IMediator mediator)
    {
        var userDisciplineIds = user.UserDisciplines.Select(ud => ud.DisciplineId).ToList();
        var userCountryId = user.CountryId ?? Guid.Empty;

        var query = new GetUserListByDefaultSearchRequestCommand
        {
            UserId = user.UserId,
            DisciplineIds = userDisciplineIds,
            CountryId = userCountryId
        };

        var result = await mediator.Send(query);
        return result;
    }

    private DefaultSearchResponseDto CreateDefaultSearchResponseDto(User user, IEnumerable<User> userList)
    {
        var userProfiles = _mapper.Map<IEnumerable<UserProfileDto>>(userList).ToList();
        var countryTitle = user.Country?.Title ?? string.Empty;

        if (user.Country == null)
        {
            _logger.LogWarning("User country is null for user ID {UserId}", user.UserId);
        }

        var teacherProfilesByCountry = GetProfilesByCountry(userProfiles, countryTitle, isTeacher: true);
        var expertProfilesByCountry = GetProfilesByCountry(userProfiles, countryTitle, isTeacher: false);
        var teacherProfilesByDisciplines =
            GetProfilesByDisciplines(userProfiles, user.UserDisciplines.Select(ud => ud.Discipline.Title), true);
        var expertProfilesByDisciplines = GetProfilesByDisciplines(userProfiles,
            user.UserDisciplines.Select(ud => ud.Discipline.Title), false);

        return new DefaultSearchResponseDto
        {
            TeacherProfilesByCountry = teacherProfilesByCountry.ToList(),
            ExpertProfilesByCountry = expertProfilesByCountry.ToList(),
            TeacherProfilesByDisciplines = teacherProfilesByDisciplines.ToList(),
            ExpertProfilesByDisciplines = expertProfilesByDisciplines.ToList(),
            ClassProfilesByCountry = GetClassProfiles(teacherProfilesByCountry).ToList(),
            ClassProfilesByDisciplines = GetClassProfiles(teacherProfilesByDisciplines).ToList()
        };
    }

    private static IEnumerable<UserProfileDto> GetProfilesByCountry(IEnumerable<UserProfileDto> userProfiles,
        string countryTitle, bool isTeacher)
    {
        var result = userProfiles.Where(up => up.CountryTitle == countryTitle && up.IsATeacher == isTeacher);
        return result;
    }

    private static IEnumerable<UserProfileDto> GetProfilesByDisciplines(IEnumerable<UserProfileDto> userProfiles,
        IEnumerable<string> disciplineTitles, bool isTeacher)
    {
        return userProfiles.Where(up =>
            up.DisciplineTitles.Intersect(disciplineTitles).Any() && up.IsATeacher == isTeacher);
    }

    private static IEnumerable<ClassProfileDto> GetClassProfiles(IEnumerable<UserProfileDto> userProfiles)
    {
        return userProfiles
            .Where(up => up.IsATeacher)
            .SelectMany(up => up.ClassDtos)
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