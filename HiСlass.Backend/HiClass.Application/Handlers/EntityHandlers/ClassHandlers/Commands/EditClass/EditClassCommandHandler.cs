using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Handlers.EntityConnectionHandlers.ClassLanguageHandlers.Commands.UpdateClassLanguages;
using HiClass.Application.Handlers.EntityHandlers.ClassHandlers.Queries.GetClass;
using HiClass.Application.Handlers.EntityHandlers.DisciplineHandlers.Queries.GetDisciplinesByTitles;
using HiClass.Application.Handlers.EntityHandlers.GradeHandlers.Queries.GetGrade;
using HiClass.Application.Handlers.EntityHandlers.LanguageHandlers.Queries.GetLanguagesByTitles;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Education;
using HiClass.Domain.Entities.Main;
using MediatR;
using static System.Threading.Tasks.Task;

namespace HiClass.Application.Handlers.EntityHandlers.ClassHandlers.Commands.EditClass;

public class EditClassCommandHandler : IRequestHandler<EditClassCommand, Class>
{
    private readonly ISharedLessonDbContext _context;
    private readonly IMediator _mediator;

    public EditClassCommandHandler(ISharedLessonDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<Class> Handle(EditClassCommand request, CancellationToken cancellationToken)
    {
        var @class =
            await _context.Classes.FindAsync(new object[] { request.ClassId }, cancellationToken: cancellationToken);

        if (@class == null)
        {
            throw new NotFoundException(nameof(Class), request.ClassId);
        }

        @class.Title = request.Title;

        var grade = await GetGrade(request.GradeNumber, cancellationToken);
        @class.GradeId = grade.GradeId;

        await Delay(20, cancellationToken);

        var discipline = await GetDiscipline(request.DisciplineTitle, cancellationToken);
        @class.DisciplineId = discipline.DisciplineId;

        await Delay(20, cancellationToken);

        var languages = await GetLanguages(request.LanguageTitles, cancellationToken);
        await _mediator.Send(new UpdateClassLanguagesCommand()
        {
            ClassId = @class.ClassId,
            NewLanguageIds = languages.Select(language =>
                language.LanguageId).ToList()
        }, cancellationToken);

        await Delay(20, cancellationToken);

        _context.Classes.Update(@class);
        await _context.SaveChangesAsync(cancellationToken);

        return await _mediator.Send(new GetClassByIdQuery(@class.ClassId), cancellationToken);
    }

    private async Task<List<Language>> GetLanguages(IEnumerable<string> languageTitles,
        CancellationToken cancellationToken)
    {
        var query = new GetLanguagesByTitlesQuery(languageTitles);
        var languages = await _mediator.Send(query, cancellationToken);

        return languages;
    }

    private async Task<Discipline> GetDiscipline(string disciplineTitles,
        CancellationToken cancellationToken)
    {
        var query = new GetDisciplinesByTitlesQuery(disciplineTitles);
        var disciplines = await _mediator.Send(query, cancellationToken);

        return disciplines[0];
    }

    private async Task<Grade> GetGrade(int gradeNumber, CancellationToken cancellationToken)
    {
        var query = new GetGradeQuery(gradeNumber);
        var grades = await _mediator.Send(query, cancellationToken);

        return grades;
    }
}