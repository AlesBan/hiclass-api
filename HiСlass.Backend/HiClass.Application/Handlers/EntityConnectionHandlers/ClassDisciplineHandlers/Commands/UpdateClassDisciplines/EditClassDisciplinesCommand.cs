using MediatR;

namespace HiClass.Application.Handlers.EntityConnectionHandlers.ClassDisciplineHandlers.Commands.
    UpdateClassDisciplines;

public class EditClassDisciplinesCommand : IRequest<Unit>
{
    public Guid ClassId { get; set; }
    public IEnumerable<Guid> NewDisciplineIds { get; set; } = null!;
}