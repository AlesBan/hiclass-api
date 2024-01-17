using MediatR;

namespace HiClass.Application.Handlers.EntityConnectionHandlers.UserDisciplinesHandlers.Commands.
    UpdateUserDisciplines;

public class UpdateUserDisciplinesCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public IEnumerable<Guid> NewDisciplineIds { get; set; } = new List<Guid>();
}