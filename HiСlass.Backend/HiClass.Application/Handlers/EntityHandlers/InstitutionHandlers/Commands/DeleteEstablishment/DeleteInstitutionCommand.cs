using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.InstitutionHandlers.Commands.DeleteEstablishment;

public class DeleteInstitutionCommand : IRequest<Unit>
{
    public Guid EstablishmentId { get; set; }
}