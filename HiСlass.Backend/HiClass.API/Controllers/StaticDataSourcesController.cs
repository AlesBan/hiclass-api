using HiClass.Application.Models.StaticData;
using HiClass.Infrastructure.InternalServices.StaticDataServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HiClass.API.Controllers;

[Authorize]
public class StaticDataSourcesController : BaseController
{
    private readonly IStaticDataService _staticDataService;

    public StaticDataSourcesController(IStaticDataService staticDataService)
    {
        _staticDataService = staticDataService;
    }

    [HttpGet("get-available-disciplines")]
    public async Task<AvailableDisciplinesDto> GetAvailableDisciplines()
    {
        return await _staticDataService.GetAvailableDisciplines(Mediator);
    }

    [HttpGet("get-available-languages")]
    public async Task<AvailableLanguagesDto> GetAvailableLanguages()
    {
        return await _staticDataService.GetAvailableLanguages(Mediator);
    }

    [HttpGet("get-available-institution-types")]
    public async Task<AvailableInstitutionTypesDto> GetAvailableInstitutionTypes()
    {
        return await _staticDataService.GetAvailableInstitutionTypes(Mediator);
    }
    
    [HttpGet("get-available-country-locations")]
    public async Task<AvailableCountriesDto> GetAllCountryLocations()
    {
        var result = await _staticDataService.GetAvailableCountries(Mediator);

        return result;
    }
}