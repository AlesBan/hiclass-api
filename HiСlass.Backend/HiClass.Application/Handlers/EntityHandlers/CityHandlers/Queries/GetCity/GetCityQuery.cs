using HiClass.Domain.Entities.Location;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.CityHandlers.Queries.GetCity;

public class GetCityQuery : IRequest<City>
{
    public string Title { get; set; }
    public Guid CountryId { get; set; }
}