using HiClass.Application.Models.User.CreateAccount;
using HiClass.Domain.Entities.Education;
using HiClass.Domain.Entities.Job;
using HiClass.Domain.Entities.Location;
using MediatR;

namespace HiClass.Application.Helpers.DataHelper;

public interface IDataForUserHelper
{
    Task<Country> GetCountryByTitle(string countryTitle, IMediator mediator);
    Task<City> GetCityByCountryId(Guid countryId, string cityTitle, IMediator mediator);
    Task<Institution> GetInstitution(CreateUserAccountRequestDto requestUserDto,
        IMediator mediator);
    Task<IEnumerable<Language>> GetLanguagesByTitles(IEnumerable<string> languages, IMediator mediator);
    Task<IEnumerable<Discipline>> GetDisciplinesByTitles(IEnumerable<string> disciplines,
        IMediator mediator);
    Task<IEnumerable<Grade>> GetGradesByNumbers(IEnumerable<int> grades, IMediator mediator);

}