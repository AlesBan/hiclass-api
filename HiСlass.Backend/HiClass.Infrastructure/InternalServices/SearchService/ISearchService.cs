using HiClass.Application.Models.Search;
using MediatR;

namespace HiClass.Infrastructure.InternalServices.SearchService;

public interface ISearchService
{
    public Task<SearchResponseDto> GetTeacherAndClassProfiles(Guid userId, SearchRequestDto searchRequest, IMediator mediator);
}