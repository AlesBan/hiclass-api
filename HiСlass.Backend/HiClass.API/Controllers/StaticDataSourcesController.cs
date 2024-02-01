using HiClass.Application.Models.StaticData;
using HiClass.Infrastructure.Services.StaticDataServices;
using Microsoft.AspNetCore.Mvc;

namespace HiClass.API.Controllers;

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
}