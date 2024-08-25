using HiClass.Domain.Entities.Main;
using MediatR;
using static System.String;

namespace HiClass.Application.Handlers.EntityHandlers.ClassHandlers.Commands.EditClass;

public class EditClassCommand : IRequest<Class>
{
    public Guid ClassId { get; set; }
    public string Title { get; set; } = Empty;
    public int GradeNumber { get; set; }
    public string DisciplineTitle { get; set; } = Empty;
    public IEnumerable<string> LanguageTitles { get; set; } = new List<string>();
}