using HiClass.Application.Dtos.SearchDtos;
using MediatR;

namespace HiClass.Infrastructure.Services.DefaultDataServices;

public interface IDefaultSearchService
{
    public Task<DefaultSearchResponseDto> GetDefaultTeacherAndClassProfiles(Guid userId, IMediator mediator);
}