using HiClass.Application.Models.Search;
using MediatR;

namespace HiClass.Infrastructure.Services.DefaultDataServices;

public interface IDefaultSearchService
{
    public Task<DefaultSearchResponseDto> GetDefaultTeacherAndClassProfiles(Guid userId, IMediator mediator);
}