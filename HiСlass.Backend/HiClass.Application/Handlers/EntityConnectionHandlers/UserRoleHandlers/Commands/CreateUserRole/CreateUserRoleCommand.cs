using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityConnectionHandlers.UserRoleHandlers.Commands.CreateUserRole;

public class CreateUserRoleCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}