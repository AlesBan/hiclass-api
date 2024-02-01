using HiClass.Application.Handlers.EntityHandlers.CountryHandlers.Queries.GetAllCountryTitles;
using HiClass.Application.Models.StaticData;
using MediatR;

namespace HiClass.Infrastructure.Services.DataBaseDataService;

public class DataBaseDataService : IDataBaseDataService
{
    public async Task<AvailableCountriesDto> GetAllCounties(IMediator mediator)
    {
        var result = await mediator.Send(new GetAllCountryTitlesQuery());

        return new AvailableCountriesDto(result);
    }
}