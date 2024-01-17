using HiClass.Domain.Entities.Job;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.InstitutionHandlers.Commands.CreateInstitution;

public class CreateInstitutionCommand : IRequest<Institution>
{
    public IEnumerable<string> Types { get; set; } = new List<string>();
    public string InstitutionTitle { get; set; }
    public string Address { get; set; }
}