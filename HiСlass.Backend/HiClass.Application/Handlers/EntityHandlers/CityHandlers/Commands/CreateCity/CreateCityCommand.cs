using HiClass.Domain.Entities.Location;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.CityHandlers.Commands.CreateCity;

public class CreateCityCommand : IRequest<City>
{
    public string Title { get; set; }
    public Guid CountryId { get; set; }
}