using MediatR;

namespace HiClass.Application.Handlers.EntityConnectionHandlers.UserGradeHandlers.Commands.CreateUserGrade;

public class CreateUserGradesCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public IEnumerable<Guid> GradeIds { get; set; }
}