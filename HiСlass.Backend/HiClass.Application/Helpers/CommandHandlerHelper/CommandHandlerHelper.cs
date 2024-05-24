using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Communication;
using HiClass.Domain.Entities.Education;
using HiClass.Domain.Entities.Job;
using HiClass.Domain.Entities.Location;
using HiClass.Domain.Entities.Main;
using HiClass.Domain.EntityConnections;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Helpers.CommandHandlerHelper;

public class CommandHandlerHelper : ICommandHandlerHelper
{
    private readonly ISharedLessonDbContext _context;
    private readonly Dictionary<Type, object> _dbSets;

    public CommandHandlerHelper(ISharedLessonDbContext context)
    {
        _context = context;
        _dbSets = new Dictionary<Type, object>
        {
            { typeof(User), _context.Users },
            { typeof(Role), _context.Roles },
            { typeof(Class), _context.Classes },
            { typeof(Grade), _context.Grades },
            { typeof(Discipline), _context.Disciplines },
            { typeof(Language), _context.Languages },
            { typeof(Institution), _context.Institutions },
            { typeof(InstitutionType), _context.InstitutionTypes },
            { typeof(City), _context.Cities },
            { typeof(Country), _context.Countries },
            { typeof(Feedback), _context.Feedbacks },
            { typeof(Invitation), _context.Invitations },
            { typeof(UserRole), _context.UserRoles },
            { typeof(UserDiscipline), _context.UserDisciplines },
            { typeof(UserGrade), _context.UserGrades },
            { typeof(UserLanguage), _context.UserLanguages },
            { typeof(ClassLanguage), _context.ClassLanguages },
            { typeof(ClassDiscipline), _context.ClassDisciplines },
            { typeof(InstitutionTypeInstitution), _context.InstitutionTypesInstitutions }
        };
    }

    public async Task<T> GetObjectFromDatabaseById<T>(Guid id) where T : class
    {
        if (_dbSets.TryGetValue(typeof(T), out var dbSet))
        {
            var entity = await ((DbSet<T>)dbSet).FindAsync(id);
            return entity;
        }

        throw new ArgumentException($"Unsupported type: {typeof(T)}");
    }
}