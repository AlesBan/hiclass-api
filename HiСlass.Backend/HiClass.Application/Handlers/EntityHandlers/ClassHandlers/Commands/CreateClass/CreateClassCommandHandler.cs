using HiClass.Application.Handlers.EntityConnectionHandlers.ClassDisciplineHandlers.Commands.CreateClassDisciplines;
using HiClass.Application.Handlers.EntityConnectionHandlers.ClassLanguageHandlers.Commands.CreateClassLanguages;
using HiClass.Application.Handlers.EntityConnectionHandlers.ClassLanguagesHandlers.Commands.CreateClassLanguages;
using HiClass.Application.Handlers.EntityHandlers.ClassHandlers.Queries.GetClass;
using HiClass.Application.Handlers.EntityHandlers.GradeHandlers.Queries.GetGrade;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.ClassHandlers.Commands.CreateClass;

public class CreateClassCommandHandler : IRequestHandler<CreateClassCommand, Class>
{
    private readonly ISharedLessonDbContext _context;
    private readonly IMediator _mediator;

    public CreateClassCommandHandler(ISharedLessonDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<Class> Handle(CreateClassCommand request, CancellationToken cancellationToken)
    {
        var newClass = await MapClass(request, cancellationToken);
        await AddClassToDb(newClass, cancellationToken);

        await SeedClassDisciplines(newClass, request);
        await SeedClassLanguages(newClass, request);
        await _context.SaveChangesAsync(cancellationToken);
        var classById = await _mediator.Send(new GetClassByIdQuery(newClass.ClassId), cancellationToken);
        return classById;
    }

    private async Task SeedClassLanguages(Class newClass, CreateClassCommand request)
    {
        await _mediator.Send(new CreateClassLanguagesCommand()
        {
            ClassId = newClass.ClassId,
            LanguageIds = request.LanguageIds
        }, CancellationToken.None);
    }

    private async Task SeedClassDisciplines(Class newClass, CreateClassCommand request)
    {
        await _mediator.Send(new CreateClassDisciplinesCommand()
        {
            ClassId = newClass.ClassId,
            DisciplineIds = request.DisciplineIds
        }, CancellationToken.None);
    }

    private async Task AddClassToDb(Class newClass, CancellationToken cancellationToken)
    {
        await _context.Classes.AddAsync(newClass, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task<Class> MapClass(CreateClassCommand request, CancellationToken cancellationToken)
    {
        var grade = await _mediator.Send(new GetGradeQuery(request.GradeNumber), cancellationToken);
        return new Class()
        {
            ClassId = request.ClassId,
            UserId = request.UserId,
            Title = request.Title,
            GradeId = grade.GradeId,
        };
    }
}