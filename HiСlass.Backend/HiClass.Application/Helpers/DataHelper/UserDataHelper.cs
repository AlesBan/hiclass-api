using HiClass.Application.Handlers.EntityHandlers.CityHandlers.Queries.GetCity;
using HiClass.Application.Handlers.EntityHandlers.CountryHandlers.Queries.GetCountryByTitle;
using HiClass.Application.Handlers.EntityHandlers.DisciplineHandlers.Queries.GetDisciplinesByTitles;
using HiClass.Application.Handlers.EntityHandlers.GradeHandlers.Queries.GetGrades;
using HiClass.Application.Handlers.EntityHandlers.InstitutionHandlers.Queries.GetInstitution;
using HiClass.Application.Handlers.EntityHandlers.LanguageHandlers.Queries.GetLanguagesByTitles;
using HiClass.Application.Models.User.CreateAccount;
using HiClass.Domain.Entities.Education;
using HiClass.Domain.Entities.Job;
using HiClass.Domain.Entities.Location;
using MediatR;

namespace HiClass.Application.Helpers.DataHelper;

public class UserDataHelper : IUserDataHelper
{
    public async Task<Country> GetCountryByTitle(string countryTitle, IMediator mediator)
    {
        var query = new GetCountryByTitleQuery(countryTitle);
        return await mediator.Send(query);
    }

    public async Task<City> GetCityByCountryId(Guid countryId, string cityTitle, IMediator mediator)
    {
        var query = new GetCityQuery()
        {
            CountryId = countryId,
            Title = cityTitle
        };

        var city = await mediator.Send(query);
        return city;
    }

    public async Task<Institution> GetInstitution(CreateUserAccountRequestDto requestUserDto, IMediator mediator)
    {
        var address = requestUserDto.InstitutionDto.Address;
        var institutionTitle = requestUserDto.InstitutionDto.Title;
        var institutionTypesTitles = requestUserDto.InstitutionDto.Types;

        var varGetQuery = new GetInstitutionQuery()
        {
            Address = address,
            InstitutionTitle = institutionTitle,
            Types = institutionTypesTitles
        };

        var institution = await mediator.Send(varGetQuery);

        return institution;
    }


    public async Task<IEnumerable<Language>> GetLanguagesByTitles(IEnumerable<string> languages, IMediator mediator)
    {
        var query = new GetLanguagesByTitlesQuery(languages);
        return await mediator.Send(query);
    }

    public async Task<IEnumerable<Discipline>> GetDisciplinesByTitles(IEnumerable<string> disciplines,
        IMediator mediator)
    {
        var query = new GetDisciplinesByTitlesQuery(disciplines);
        return await mediator.Send(query);
    }

    public async Task<IEnumerable<Grade>> GetGradesByNumbers(IEnumerable<int> grades, IMediator mediator)
    {
        var query = new GetGradesQuery(grades);
        return await mediator.Send(query);
    }
}