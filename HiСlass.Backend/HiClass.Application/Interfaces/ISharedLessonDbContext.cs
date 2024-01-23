using HiClass.Domain.Entities.Communication;
using HiClass.Domain.Entities.Education;
using HiClass.Domain.Entities.Job;
using HiClass.Domain.Entities.Location;
using HiClass.Domain.Entities.Main;
using HiClass.Domain.EntityConnections;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Interfaces;

public interface ISharedLessonDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<Role> Roles { get; set; }
    DbSet<Grade> Grades { get; set; }
    DbSet<Class> Classes { get; set; }
    DbSet<Discipline> Disciplines { get; set; }
    DbSet<Language> Languages { get; set; }
    DbSet<Institution> Institutions { get; set; }
    DbSet<InstitutionType> InstitutionTypes { get; set; }
    DbSet<City> Cities { get; set; }
    DbSet<Country> Countries { get; set; }
    DbSet<Feedback> Feedbacks { get; set; }
    DbSet<Invitation> Invitations { get; set; }
    DbSet<UserRole> UserRoles { get; set; }
    DbSet<UserDiscipline> UserDisciplines { get; set; }
    DbSet<UserGrade> UserGrades { get; set; }
    DbSet<UserLanguage> UserLanguages { get; set; }
    DbSet<ClassLanguage> ClassLanguages { get; set; }
    DbSet<ClassDiscipline> ClassDisciplines { get; set; }
    DbSet<InstitutionTypeInstitution> InstitutionTypesInstitutions { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}