using System.ComponentModel.DataAnnotations;
using HiClass.Domain.Entities.Education;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.DisciplineHandlers.Queries.GetDisciplineByTitle;

public class GetDisciplineByTitleQuery : IRequest<Discipline>
{
    [Required] public string DisciplineTitle { get; set; }
    
    public GetDisciplineByTitleQuery(string disciplineTitle)
    {
        DisciplineTitle = disciplineTitle;
    }
}