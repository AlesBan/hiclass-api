using HiClass.Application.Models.StaticData;
using MediatR;

namespace HiClass.Infrastructure.Services.DataBaseDataService;

public interface IDataBaseDataService
{
    Task<AvailableCountriesDto> GetAllCounties(IMediator mediator);
}