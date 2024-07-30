using HiClass.API.Filters.Abilities;
using HiClass.API.Filters.UserVerification;
using HiClass.Application.Models.Search;
using HiClass.Infrastructure.InternalServices.SearchServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HiClass.API.Controllers;

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

    [Authorize]
    [CheckUserVerification]
    [CheckUserCreateAccount]
    [HttpGet("search-request")]
    public async Task<SearchResponseDto> GetTeachersAndClassesDependingOnSearchRequest(
        [FromQuery] SearchRequestDto searchRequest)
    {
        return await _searchService.GetTeacherAndClassProfiles(UserId, searchRequest, Mediator);
    }

    [Authorize]
    [CheckUserVerification]
    [CheckUserCreateAccount]
    [HttpGet("default-search-request")]
    public async Task<DefaultSearchResponseDto> GetTeachersAndClassesDependingOnDefaultSearchRequest()
    {
        return await _defaultSearchDataService.GetDefaultTeacherAndClassProfiles(UserId, Mediator);
    }
}