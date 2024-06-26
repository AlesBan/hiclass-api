using HiClass.Domain.Entities.Education;
using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityConnectionHandlers.UserDisciplinesHandlers.Commands.DeleteUserDisciplines;

public class DeleteUserDisciplineCommand : IRequest<Unit>
{
    public User User { get; set; }
    public Discipline Discipline { get; set; }
}