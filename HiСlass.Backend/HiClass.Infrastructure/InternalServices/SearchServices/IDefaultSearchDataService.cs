using HiClass.Application.Models.Search;
using MediatR;

namespace HiClass.Infrastructure.InternalServices.SearchServices;

public interface IDefaultSearchService
{
    public Task<DefaultSearchResponseDto> GetDefaultTeacherAndClassProfiles(Guid userId, IMediator mediator);
}