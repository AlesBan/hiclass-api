using HiClass.Domain.Entities.Education;
using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityConnectionHandlers.UserGradeHandlers.Commands.DeleteUserGrade;

public class DeleteUserGradeCommand : IRequest<Unit>
{
    public User User { get; set; }
    public Grade Grade { get; set; }
}