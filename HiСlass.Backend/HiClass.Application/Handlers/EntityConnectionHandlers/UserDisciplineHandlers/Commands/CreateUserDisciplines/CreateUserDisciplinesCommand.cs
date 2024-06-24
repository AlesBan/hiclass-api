using MediatR;

namespace HiClass.Application.Handlers.EntityConnectionHandlers.UserDisciplinesHandlers.Commands.CreateUserDisciplines;

public class CreateUserDisciplinesCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public IEnumerable<Guid> DisciplineIds { get; set; }
}