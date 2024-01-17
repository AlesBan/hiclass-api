using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityConnectionHandlers.UserRoleHandlers.Commands.CreateUserRole;

public class CreateUserRoleCommand : IRequest<Unit>
{
    public User User { get; set; }
    public Role Role { get; set; }
}