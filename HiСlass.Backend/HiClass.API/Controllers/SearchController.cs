using HiClass.API.Filters;
using HiClass.Application.Dtos.SearchDtos;
using HiClass.Application.Dtos.SearchDtos.Data;
using HiClass.Infrastructure.Services.DataBaseDataService;
using HiClass.Infrastructure.Services.DefaultDataServices;
using HiClass.Infrastructure.Services.SearchService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HiClass.API.Controllers;

[Authorize]
[CheckUserCreateAccount]
[Produces("application/json")]
public class SearchController : BaseController
{
    private readonly IDefaultSearchService _defaultSearchDataService;
    private readonly ISearchService _searchService;
    private readonly IDataBaseDataService _searchDataService;

    public SearchController(IDefaultSearchService defaultSearchDataService, ISearchService searchService,
        IDataBaseDataService searchDataService)
    {
        _defaultSearchDataService = defaultSearchDataService;
        _searchService = searchService;
        _searchDataService = searchDataService;
    }

    [HttpGet("search-request")]
    public async Task<SearchResponseDto> GetTeachersAndClassesDependingOnSearchRequest(
        [FromQuery] SearchRequestDto searchRequest)
    {
        return await _searchService.GetTeacherAndClassProfiles(UserId, searchRequest, Mediator);
    }

    [HttpGet("default-search-request")]
    public async Task<DefaultSearchResponseDto> GetTeachersAndClassesDependingOnDefaultSearchRequest()
    {
        return await _defaultSearchDataService.GetDefaultTeacherAndClassProfiles(UserId, Mediator);
    }

    [HttpGet("all-country-locations")]
    public async Task<ExistingCountriesDto> GetAllCountryLocations()
    {
        var result = await _searchDataService.GetAllCounties(Mediator);

        return result;
    }
}