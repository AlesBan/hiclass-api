using HiClass.Application.Models.StaticData;
using MediatR;

namespace HiClass.Application.Interfaces.Services;

public interface IDataBaseDataService
{
    Task<AvailableCountriesDto> GetAllCounties(IMediator mediator);
}