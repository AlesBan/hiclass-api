using HiClass.Domain.Entities.Education;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.DisciplineHandlers.Queries.GetDisciplinesByTitles;

public class GetDisciplinesByTitlesQuery : IRequest<List<Discipline>>
{
    public IEnumerable<string> DisciplineTitles { get; set; }

    public GetDisciplinesByTitlesQuery(IEnumerable<string> disciplineTitles)
    {
        DisciplineTitles = disciplineTitles;
    }

    public GetDisciplinesByTitlesQuery(string disciplineTitle)
    {
        DisciplineTitles = new List<string>() { disciplineTitle };
    }
}