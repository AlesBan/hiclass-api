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
            User = request.User,
            Role = request.Role
        };

        await _context.UserRoles.AddAsync(userRole, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}