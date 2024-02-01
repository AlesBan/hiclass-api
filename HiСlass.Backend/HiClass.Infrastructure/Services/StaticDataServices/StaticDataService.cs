using HiClass.Application.Handlers.EntityHandlers.DisciplineHandlers.Queries.GetAllDisciplineTitles;
using HiClass.Application.Handlers.EntityHandlers.InstitutionHandlers.Queries.GetAllInstitutionTypes;
using HiClass.Application.Handlers.EntityHandlers.LanguageHandlers.Queries.GetAllLanguageTitles;
using HiClass.Application.Models.StaticData;
using MediatR;

namespace HiClass.Infrastructure.Services.StaticDataServices;

public class StaticDataService : IStaticDataService
{
    public async Task<AvailableDisciplinesDto> GetAvailableDisciplines(IMediator mediator)
    {
        var query = new GetAllDisciplineTitlesQuery();
        var result = await mediator.Send(query);
        
        var availableDisciplinesDto = new AvailableDisciplinesDto()
        {
            AvailableDisciplines = result
        };
        return availableDisciplinesDto;
    }

    public async Task<AvailableLanguagesDto> GetAvailableLanguages(IMediator mediator)
    {
        var query = new GetAllLanguageTitlesQuery();
        var result = await mediator.Send(query);

        var availableLanguagesDto = new AvailableLanguagesDto()
        {
            AvailableLanguages = result
        };

        return availableLanguagesDto;
    }

    public async Task<AvailableInstitutionTypesDto> GetAvailableInstitutionTypes(IMediator mediator)
    {
        var query = new GetAllInstitutionTypesQuery();
        var result = await mediator.Send(query);

        var availableInstitutionTypesDto = new AvailableInstitutionTypesDto()
        {
            AvailableInstitutionTypes = result
        };

        return availableInstitutionTypesDto;
    }
}