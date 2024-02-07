using HiClass.Application.Models.Search;
using MediatR;

namespace HiClass.Application.Interfaces.Services;

public interface IDefaultSearchService
{
    public Task<DefaultSearchResponseDto> GetDefaultTeacherAndClassProfiles(Guid userId, IMediator mediator);
}