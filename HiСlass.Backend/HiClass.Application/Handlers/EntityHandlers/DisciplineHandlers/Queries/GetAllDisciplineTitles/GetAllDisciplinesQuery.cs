using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.DisciplineHandlers.Queries.GetAllDisciplineTitles;

public class GetAllDisciplineTitlesQuery : IRequest<List<string>>
{
}