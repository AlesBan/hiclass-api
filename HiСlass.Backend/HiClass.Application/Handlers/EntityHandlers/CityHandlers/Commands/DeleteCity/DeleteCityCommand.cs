using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.CityHandlers.Commands.DeleteCity;

public class DeleteCityCommand : IRequest<Unit>
{
    public Guid CityId { get; set; }
}