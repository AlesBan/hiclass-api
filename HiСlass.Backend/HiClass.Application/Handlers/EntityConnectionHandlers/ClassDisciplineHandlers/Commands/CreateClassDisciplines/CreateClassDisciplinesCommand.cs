using MediatR;

namespace HiClass.Application.Handlers.EntityConnectionHandlers.ClassDisciplineHandlers.Commands.
    CreateClassDisciplines;

public class CreateClassDisciplinesCommand : IRequest<Unit>
{
    public Guid ClassId { get; set; }
    public IEnumerable<Guid> DisciplineIds { get; set; } = null!;
}