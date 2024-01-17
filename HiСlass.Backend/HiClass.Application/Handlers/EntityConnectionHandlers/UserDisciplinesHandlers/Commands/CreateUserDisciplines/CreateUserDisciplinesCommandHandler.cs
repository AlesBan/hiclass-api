using HiClass.Application.Interfaces;
using HiClass.Domain.EntityConnections;
using MediatR;

namespace HiClass.Application.Handlers.EntityConnectionHandlers.UserDisciplinesHandlers.Commands.
    CreateUserDisciplines;

public class CreateUserDisciplinesCommandHandler : IRequestHandler<CreateUserDisciplinesCommand, Unit>
{
    private readonly ISharedLessonDbContext _context;

    public CreateUserDisciplinesCommandHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(CreateUserDisciplinesCommand request, CancellationToken cancellationToken)
    {
        var userDisciplines = (from disciplineId in request.DisciplineIds 
            let exists = _context.UserDisciplines.Any(d => 
                d.DisciplineId == disciplineId && d.UserId == request.UserId) 
            where !exists 
            select new UserDiscipline() 
                { UserId = request.UserId, DisciplineId = disciplineId })
            .ToList();

        await _context.UserDisciplines
            .AddRangeAsync(userDisciplines, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}