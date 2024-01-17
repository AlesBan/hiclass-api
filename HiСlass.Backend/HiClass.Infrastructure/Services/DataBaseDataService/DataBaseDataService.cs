using HiClass.Application.Dtos.SearchDtos.Data;
using HiClass.Application.Handlers.EntityHandlers.CountryHandlers.Queries.GetAllCountryTitles;
using MediatR;

namespace HiClass.Infrastructure.Services.DataBaseDataService;

public class DataBaseDataService : IDataBaseDataService
{
    public async Task<ExistingCountriesDto> GetAllCounties(IMediator mediator)
    {
        var result = await mediator.Send(new GetAllCountryTitlesQuery());

        return new ExistingCountriesDto(result);
    }
}