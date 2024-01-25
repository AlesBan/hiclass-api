using HiClass.Application.Dtos.StaticDataDtos;
using MediatR;

namespace HiClass.Infrastructure.Services.StaticDataServices;

public interface IStaticDataService
{
    public Task<AvailableDisciplinesDto> GetAvailableDisciplines(IMediator mediator);
    public Task<AvailableLanguagesDto> GetAvailableLanguages(IMediator mediator);
    public Task<AvailableInstitutionTypesDto> GetAvailableInstitutionTypes(IMediator mediator);
}