using HiClass.Application.Interfaces;
using HiClass.Domain.EntityConnections;
using MediatR;

namespace HiClass.Application.Handlers.EntityConnectionHandlers.UserRoleHandlers.Commands.CreateUserRole;

public class CreateUserRoleCommandHandler : IRequestHandler<CreateUserRoleCommand, Unit>
{
    private readonly ISharedLessonDbContext _context;

    public CreateUserRoleCommandHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(CreateUserRoleCommand request, CancellationToken cancellationToken)
    {
        var userRole = new UserRole()
        {
            UserId = request.UserId,
            RoleId = request.RoleId
        };

        await _context.UserRoles.AddAsync(userRole, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}