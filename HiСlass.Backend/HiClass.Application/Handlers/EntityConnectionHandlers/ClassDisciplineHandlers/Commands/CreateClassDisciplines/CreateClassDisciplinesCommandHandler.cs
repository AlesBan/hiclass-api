using System.Runtime.ConstrainedExecution;
using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Education;
using HiClass.Domain.EntityConnections;
using MediatR;

namespace HiClass.Application.Handlers.EntityConnectionHandlers.ClassDisciplineHandlers.Commands.
    CreateClassDisciplines;

public class CreateClassDisciplinesCommandHandler : IRequestHandler<CreateClassDisciplinesCommand, Unit>
{
    private readonly ISharedLessonDbContext _context;

    public CreateClassDisciplinesCommandHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(CreateClassDisciplinesCommand request, CancellationToken cancellationToken)
    {
        var classDisciplines = request.DisciplineIds
            .Select(discipline =>
                new ClassDiscipline()
                {
                    ClassId = request.ClassId,
                    DisciplineId = discipline
                });
        try
        {
            await _context.ClassDisciplines
                .AddRangeAsync(classDisciplines, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            throw new NotFoundException(nameof(Discipline), request.DisciplineIds);
        }

        return Unit.Value;
    }
}