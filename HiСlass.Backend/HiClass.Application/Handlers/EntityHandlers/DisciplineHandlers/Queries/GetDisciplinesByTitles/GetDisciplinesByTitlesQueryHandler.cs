using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Education;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.DisciplineHandlers.Queries.GetDisciplinesByTitles;

public class GetDisciplinesByTitlesQueryHandler : IRequestHandler<GetDisciplinesByTitlesQuery, List<Discipline>>
{
    private readonly ISharedLessonDbContext _context;

    public GetDisciplinesByTitlesQueryHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public Task<List<Discipline>> Handle(GetDisciplinesByTitlesQuery request, CancellationToken cancellationToken)
    {
        var requiredDisciplines = request.DisciplineTitles;
        var disciplines = _context.Disciplines
            .Where(d => requiredDisciplines.Contains(d.Title))
            .ToList();

        foreach (var disciplineTitle in requiredDisciplines.Where(discipline =>
                     !disciplines.Select(d => d.Title).Contains(discipline)))
        {
            throw new NotFoundException(nameof(Discipline), disciplineTitle);
        }

        return Task.FromResult(disciplines.ToList());
    }
}