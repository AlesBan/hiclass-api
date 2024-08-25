using HiClass.Application.Common.Exceptions.User;
using HiClass.Application.Common.Exceptions.User.EditUser;
using HiClass.Application.Handlers.EntityConnectionHandlers.UserDisciplinesHandlers.Commands.UpdateUserDisciplines;
using HiClass.Application.Handlers.EntityConnectionHandlers.UserGradeHandlers.Commands.UpdateUserGrades;
using HiClass.Application.Handlers.EntityConnectionHandlers.UserLanguagesHandlers.Commands.UpdateUserLanguages;
using HiClass.Application.Handlers.EntityHandlers.DisciplineHandlers.Queries.GetDisciplinesByTitles;
using HiClass.Application.Handlers.EntityHandlers.GradeHandlers.Queries.GetGrades;
using HiClass.Application.Handlers.EntityHandlers.LanguageHandlers.Queries.GetLanguagesByTitles;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Education;
using HiClass.Domain.Entities.Main;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.EditProfessionalInfo;

public class EditProfessionalInfoCommandHandler : IRequestHandler<EditProfessionalInfoCommand, User>
{
    private readonly ISharedLessonDbContext _context;
    private readonly IMediator _mediator;

    public EditProfessionalInfoCommandHandler(ISharedLessonDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<User> Handle(EditProfessionalInfoCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(u => u.Classes)
            .ThenInclude(c => c.ClassLanguages)
            .ThenInclude(cl => cl.Language)
            .Include(u => u.Classes)
            .ThenInclude(cd => cd.Discipline)
            .Include(u => u.Classes)
            .ThenInclude(c => c.Grade)
            .AsNoTracking()
            .FirstOrDefaultAsync(u =>
                u.UserId == request.UserId, cancellationToken: cancellationToken);

        if (user == null)
        {
            throw new UserNotFoundByIdException(request.UserId);
        }

        var newLanguages = await GetLanguages(request.LanguageTitles, cancellationToken);

        var newDisciplines = await GetDisciplines(request.DisciplineTitles, cancellationToken);

        var newGrades = await GetGrades(request.GradeNumbers, cancellationToken);
        
        foreach (var @class in user.Classes)
        {
            var languageMatchFound = false;
            var missingLanguageTitle = "";

            foreach (var newLanguage in newLanguages.Where(newLanguage =>
                         @class.ClassLanguages.All(cl => cl.LanguageId != newLanguage.LanguageId)))
            {
                languageMatchFound = true;
                missingLanguageTitle = newLanguage.Title;
                break;
            }

            if (!languageMatchFound)
            {
                throw new MissingClassLanguageException(@class.ClassId, missingLanguageTitle);
            }

            var disciplineMatchFound = false;
            var missingDisciplineTitle = "";

            foreach (var newDiscipline in newDisciplines)
            {
                if (newDiscipline.DisciplineId == @class.DisciplineId)
                {
                    disciplineMatchFound = true;
                    break; 
                }
                missingDisciplineTitle = newDiscipline.Title;
            }

            if (!disciplineMatchFound)
            {
                throw new MissingClassDisciplineException(@class.ClassId, missingDisciplineTitle);
            }

            var gradeMatchFound = false;
            var missingGradeNumber = 0;

            foreach (var newGrade in newGrades)
            {
                if (newGrade.GradeId == @class.GradeId)
                {
                    gradeMatchFound = true;
                    break; 
                }

                missingGradeNumber = newGrade.GradeNumber;
            }

            if (!gradeMatchFound)
            {
                throw new MissingClassGradeException(@class.ClassId, missingGradeNumber);
            }
        }

        var updateUserLanguagesQuery = new UpdateUserLanguagesCommand
        {
            UserId = user.UserId,
            NewLanguageIds = newLanguages.Select(l => l.LanguageId).ToList()
        };

        var updateUserDisciplinesQuery = new UpdateUserDisciplinesCommand
        {
            UserId = user.UserId,
            NewDisciplineIds = newDisciplines.Select(d => d.DisciplineId).ToList()
        };

        var updateUserGradesQuery = new UpdateUserGradesCommand()
        {
            UserId = user.UserId,
            NewGradeIds = newGrades.Select(g => g.GradeId).ToList()
        };

        await _mediator.Send(updateUserLanguagesQuery, cancellationToken);
        await _mediator.Send(updateUserDisciplinesQuery, cancellationToken);
        await _mediator.Send(updateUserGradesQuery, cancellationToken);

        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);

        user = await _context.Users
            .Include(u => u.City)
            .Include(u => u.Country)
            .Include(u => u.Institution)
            .Include(u => u.Classes)
            .ThenInclude(c => c.ClassLanguages)
            .ThenInclude(cl => cl.Language)
            .Include(u => u.Classes)
            .ThenInclude(cd => cd.Discipline)
            .Include(u => u.Classes)
            .ThenInclude(c => c.Grade)
            .Include(u => u.UserDisciplines)
            .ThenInclude(ud => ud.Discipline)
            .Include(u => u.UserLanguages)
            .ThenInclude(ul => ul.Language)
            .Include(u => u.UserGrades)
            .ThenInclude(ug => ug.Grade)
            .FirstOrDefaultAsync(u =>
                u.UserId == request.UserId, cancellationToken: cancellationToken);

        return user;
    }

    private async Task<List<Language>> GetLanguages(IEnumerable<string> languageTitles,
        CancellationToken cancellationToken)
    {
        var query = new GetLanguagesByTitlesQuery(languageTitles);
        var languages = await _mediator.Send(query, cancellationToken);

        return languages;
    }

    private async Task<List<Discipline>> GetDisciplines(IEnumerable<string> disciplineTitles,
        CancellationToken cancellationToken)
    {
        var query = new GetDisciplinesByTitlesQuery(disciplineTitles);
        var disciplines = await _mediator.Send(query, cancellationToken);

        return disciplines;
    }

    private async Task<List<Grade>> GetGrades(IEnumerable<int> gradeNumbers, CancellationToken cancellationToken)
    {
        var query = new GetGradesQuery(gradeNumbers);
        var grades = await _mediator.Send(query, cancellationToken);

        return grades;
    }
}