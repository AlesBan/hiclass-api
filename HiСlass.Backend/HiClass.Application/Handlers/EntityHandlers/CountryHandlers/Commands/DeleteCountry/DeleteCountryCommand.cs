using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.CountryHandlers.Commands.DeleteCountry;

public class DeleteCountryCommand : IRequest<Unit>
{
    public Guid CountryId { get; set; }
}