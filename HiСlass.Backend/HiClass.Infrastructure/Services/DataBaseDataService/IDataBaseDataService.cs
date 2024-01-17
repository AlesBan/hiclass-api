using HiClass.Application.Dtos.SearchDtos.Data;
using MediatR;

namespace HiClass.Infrastructure.Services.DataBaseDataService;

public interface IDataBaseDataService
{
    Task<ExistingCountriesDto> GetAllCounties(IMediator mediator);
}