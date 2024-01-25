using HiClass.Application.Interfaces;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.DisciplineHandlers.Queries.GetAllDisciplineTitles;

public class GetAllDisciplineTitlesQueryHandler: IRequestHandler<GetAllDisciplineTitlesQuery, List<string>>
{
    private readonly ISharedLessonDbContext _context;

    public GetAllDisciplineTitlesQueryHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public Task<List<string>> Handle(GetAllDisciplineTitlesQuery request, CancellationToken cancellationToken)
    {
        var titles = _context.Disciplines.Select(d => d.Title).ToList();

        return Task.FromResult(titles);
    }
}