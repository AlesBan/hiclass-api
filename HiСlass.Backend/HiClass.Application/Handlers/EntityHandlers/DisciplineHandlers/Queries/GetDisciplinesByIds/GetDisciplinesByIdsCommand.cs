using HiClass.Domain.Entities.Education;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.DisciplineHandlers.Queries.
    GetDisciplinesByIds;

public class GetDisciplinesByIdsCommand : IRequest<List<Discipline>>
{
    public IEnumerable<Guid> DisciplineIds { get; set; }

    public GetDisciplinesByIdsCommand(IEnumerable<Guid> disciplineIds)
    {
        DisciplineIds = disciplineIds;
    }
}