using HiClass.Application.Models.Search;
using MediatR;

namespace HiClass.Infrastructure.Services.SearchService;

public interface ISearchService
{
    /// <summary>
    /// GetTeacherAndClassProfilesDependingOnSearchRequest
    /// </summary>
    /// <param name="searchRequest"></param>
    /// <param name="mediator"></param>
    /// <returns></returns>
    public Task<SearchResponseDto> GetTeacherAndClassProfiles(Guid userId, SearchRequestDto searchRequest, IMediator mediator);
}