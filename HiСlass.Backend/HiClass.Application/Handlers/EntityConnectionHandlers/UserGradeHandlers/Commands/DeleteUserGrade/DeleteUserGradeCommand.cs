using HiClass.Domain.Entities.Education;
using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityConnectionHandlers.UserGradeHandlers.Commands.DeleteUserGrade;

public class DeleteUserGradeCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public Guid GradeId { get; set; }
}