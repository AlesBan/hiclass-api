using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Education;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.DisciplineHandlers.Queries.GetDisciplineByTitle;

public class GetDisciplineByTitleQueryHandler : IRequestHandler<GetDisciplineByTitleQuery, Discipline>
{
    private readonly ISharedLessonDbContext _context;

    public GetDisciplineByTitleQueryHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public Task<Discipline> Handle(GetDisciplineByTitleQuery request, CancellationToken cancellationToken)
    {
        var discipline = _context.Disciplines
            .FirstOrDefault(d => d.Title == request.DisciplineTitle);
        if (discipline == null)
        {
            throw new NotFoundException(nameof(Discipline), request.DisciplineTitle);
        }

        return Task.FromResult(discipline);
    }
}