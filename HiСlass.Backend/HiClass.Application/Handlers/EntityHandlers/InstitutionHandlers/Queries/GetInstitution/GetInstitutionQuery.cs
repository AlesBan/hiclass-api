using HiClass.Domain.Entities.Job;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.InstitutionHandlers.Queries.GetInstitution;

public class GetInstitutionQuery : IRequest<Institution>
{
    public string Address { get; set; }
    public string InstitutionTitle { get; set; } = string.Empty;
    public IEnumerable<string> Types { get; set; } = new List<string>();
}