using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.ClassHandlers.Commands.EditClass;

public class EditClassCommand : IRequest<Class>
{
    public Guid ClassId { get; set; }
    public string Title { get; set; }
    public int GradeNumber { get; set; }
    public IEnumerable<string> DisciplineTitles { get; set; } = new List<string>();
    public IEnumerable<string> LanguageTitles { get; set; } = new List<string>();
}