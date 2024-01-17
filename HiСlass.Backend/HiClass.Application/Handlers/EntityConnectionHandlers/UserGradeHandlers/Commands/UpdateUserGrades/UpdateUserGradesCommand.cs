using MediatR;

namespace HiClass.Application.Handlers.EntityConnectionHandlers.UserGradeHandlers.Commands.UpdateUserGrades;

public class UpdateUserGradesCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public ICollection<Guid> NewGradeIds { get; set; } = new List<Guid>();
}