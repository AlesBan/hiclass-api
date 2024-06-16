using HiClass.API.Filters;
using HiClass.API.Filters.Abilities;
using HiClass.API.Filters.UserVerification;
using HiClass.Application.Interfaces.Services;
using HiClass.Application.Models.Search;
using HiClass.Application.Models.StaticData;
using HiClass.Infrastructure.Services.DataBaseDataService;
using HiClass.Infrastructure.Services.DefaultDataServices;
using HiClass.Infrastructure.Services.SearchService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HiClass.API.Controllers;

[Authorize]
[CheckUserVerification]
[CheckUserCreateAccount]
[Produces("application/json")]
public class SearchController : BaseController
{
    private readonly IDefaultSearchService _defaultSearchDataService;
    private readonly ISearchService _searchService;

    public SearchController(IDefaultSearchService defaultSearchDataService, ISearchService searchService)
    {
        _defaultSearchDataService = defaultSearchDataService;
        _searchService = searchService;
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
}