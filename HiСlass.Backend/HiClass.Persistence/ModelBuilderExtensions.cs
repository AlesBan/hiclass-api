using HiClass.Application.Helpers;
using HiClass.Domain.Entities.Education;
using HiClass.Domain.Entities.Main;
using HiClass.Domain.Enums.EntityTypes;
using HiClass.Persistence.EntityConfiguration.Communication;
using HiClass.Persistence.EntityConfiguration.Education;
using HiClass.Persistence.EntityConfiguration.Job;
using HiClass.Persistence.EntityConfiguration.Location;
using HiClass.Persistence.EntityConfiguration.Main;
using HiClass.Persistence.EntityConnectionsConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using InstitutionType = HiClass.Domain.Entities.Job.InstitutionType;

namespace HiClass.Persistence;

public static class ModelBuilderExtensions
{
    public static void AppendConfigurations(this ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());

        modelBuilder.ApplyConfiguration(new ClassConfiguration());
        modelBuilder.ApplyConfiguration(new GradeConfiguration());
        modelBuilder.ApplyConfiguration(new DisciplineConfiguration());
        modelBuilder.ApplyConfiguration(new LanguageConfiguration());

        modelBuilder.ApplyConfiguration(new InstitutionConfiguration());
        modelBuilder.ApplyConfiguration(new InstitutionTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CityConfiguration());
        modelBuilder.ApplyConfiguration(new CountryConfiguration());
        modelBuilder.ApplyConfiguration(new FeedbackConfiguration());
        modelBuilder.ApplyConfiguration(new InvitationConfiguration());

        modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
        modelBuilder.ApplyConfiguration(new UserGradeConfiguration());
        modelBuilder.ApplyConfiguration(new UserDisciplineConfiguration());
        modelBuilder.ApplyConfiguration(new UserLanguageConfiguration());
        modelBuilder.ApplyConfiguration(new ClassDisciplineConfiguration());
        modelBuilder.ApplyConfiguration(new ClassLanguageConfiguration());
        modelBuilder.ApplyConfiguration(new InstitutionTypeInstitutionConfiguration());
    }

    public static void SeedingDefaultData(this ModelBuilder modelBuilder, IConfiguration configuration)
    {
        modelBuilder.SeedingRoles();
        modelBuilder.SeedingGrades();
        modelBuilder.SeedingDisciplines(configuration);
        modelBuilder.SeedingLanguages(configuration);
        modelBuilder.SeedingEstablishmentTypes(configuration);
    }

    private static void SeedingGrades(this ModelBuilder modelBuilder)
    {
        var grades = Enumerable.Range(1, 12)
            .Select(i => new Grade()
            {
                GradeId = Guid.NewGuid(),
                GradeNumber = i
            })
            .ToList();

        modelBuilder.Entity<Grade>()
            .HasData(grades);
    }

    private static void SeedingDisciplines(this ModelBuilder modelBuilder, IConfiguration configuration)
    {
        var filePath = configuration["STATIC_DATA_SOURCES:DISCIPLINES_FILEPATH"];
        var disciplines = FileHelper.GetLines(filePath);
        modelBuilder.Entity<Discipline>().HasData(disciplines
            .Select(d =>
                new Discipline
                {
                    DisciplineId = Guid.NewGuid(),
                    Title = d.ToString()
                }).ToList());
    }

    private static void SeedingLanguages(this ModelBuilder modelBuilder, IConfiguration configuration)
    {
        var filePath = configuration["STATIC_DATA_SOURCES:LANGUAGES_FILEPATH"];
        var languages = FileHelper.GetLines(filePath);
        modelBuilder.Entity<Language>().HasData(languages
            .Select(l =>
                new Language
                {
                    LanguageId = Guid.NewGuid(),
                    Title = l.ToString()
                }).ToList());
    }

    private static void SeedingRoles(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>()
            .HasData(((RoleTypes[])
                    Enum.GetValues(typeof(RoleTypes)))
                .Select(r =>
                    new Role()
                    {
                        RoleId = Guid.NewGuid(),
                        Title = r.ToString()
                    }).ToList());
    }

    private static void SeedingEstablishmentTypes(this ModelBuilder modelBuilder, IConfiguration configuration)
    {
        var filePath = configuration["STATIC_DATA_SOURCES:INSTITUTIONTYPES_FILEPATH"];
        var institutions = FileHelper.GetLines(filePath);
        modelBuilder.Entity<InstitutionType>().HasData(institutions
            .Select(l =>
                new InstitutionType
                {
                    InstitutionTypeId = Guid.NewGuid(),
                    Title = l.ToString()
                }).ToList());
    }
}