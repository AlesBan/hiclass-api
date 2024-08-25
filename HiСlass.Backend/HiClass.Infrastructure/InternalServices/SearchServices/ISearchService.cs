using HiClass.Application.Models.Search;
using MediatR;

namespace HiClass.Infrastructure.InternalServices.SearchServices;

public interface ISearchService
{
    public Task<SearchResponseDto> GetTeacherAndClassProfiles(Guid userId, SearchRequestDto searchRequest, IMediator mediator);
}