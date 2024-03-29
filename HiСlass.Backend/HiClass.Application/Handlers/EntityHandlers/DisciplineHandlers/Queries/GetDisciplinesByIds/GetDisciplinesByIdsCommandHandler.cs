using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Education;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.DisciplineHandlers.Queries.GetDisciplinesByIds;

public class GetDisciplinesByIdsCommandHandler : IRequestHandler<GetDisciplinesByIdsCommand, List<Discipline>>
{
    private readonly ISharedLessonDbContext _context;

    public GetDisciplinesByIdsCommandHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public Task<List<Discipline>> Handle(GetDisciplinesByIdsCommand request, CancellationToken cancellationToken)
    {
        var requestDisciplineIds = request.DisciplineIds;
        var disciplines = _context.Disciplines.Where(d =>
            requestDisciplineIds.Contains(d.DisciplineId));

        return Task.FromResult(disciplines.ToList());
    }
}