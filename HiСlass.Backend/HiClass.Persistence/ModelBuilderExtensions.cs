using HiClass.Application.Helpers;
using HiClass.Domain.Entities.Education;
using HiClass.Domain.Entities.Job;
using HiClass.Domain.Entities.Location;
using HiClass.Domain.Entities.Main;
using HiClass.Domain.EntityConnections;
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
    private static Dictionary<string, Guid> DictionaryDisplineIds { get; set; } = new();
    private static Dictionary<string, Guid> DictionaryLanguageIds { get; set; } = new();
    private static Dictionary<string, Guid> DictionaryGradeIds { get; set; } = new();

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

        // modelBuilder.SeedingUsers();
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

        var disciplineList = disciplines
            .Select(d => new Discipline
            {
                DisciplineId = Guid.NewGuid(),
                Title = d.ToString()
            })
            .ToList();
        
        DictionaryDisplineIds.Clear();
        foreach (var discipline in disciplineList)
        {
            DictionaryDisplineIds.Add(discipline.Title, discipline.DisciplineId);
        }

        modelBuilder.Entity<Discipline>().HasData(disciplineList);
    }

    private static void SeedingLanguages(this ModelBuilder modelBuilder, IConfiguration configuration)
    {
        var filePath = configuration["STATIC_DATA_SOURCES:LANGUAGES_FILEPATH"];
        var languages = FileHelper.GetLines(filePath);
        var languageList = languages
            .Select(l => new Language
            {
                LanguageId = Guid.NewGuid(),
                Title = l.ToString()
            })
            .ToList();
        modelBuilder.Entity<Language>().HasData(languageList);
    }

    private static void SeedingRoles(this ModelBuilder modelBuilder)
    {
        var roleList = ((RoleTypes[])Enum.GetValues(typeof(RoleTypes)))
            .Select(r => new Role
            {
                RoleId = Guid.NewGuid(),
                Title = r.ToString()
            })
            .ToList();
        modelBuilder.Entity<Role>().HasData(roleList);
    }

    private static void SeedingEstablishmentTypes(this ModelBuilder modelBuilder, IConfiguration configuration)
    {
        var filePath = configuration["STATIC_DATA_SOURCES:INSTITUTIONTYPES_FILEPATH"];
        var institutionTypes = FileHelper.GetLines(filePath);
        var institutionTypeList = institutionTypes
            .Select(l => new InstitutionType
            {
                InstitutionTypeId = Guid.NewGuid(),
                Title = l.ToString()
            })
            .ToList();
        modelBuilder.Entity<InstitutionType>().HasData(institutionTypeList);
    }

    private static void SeedingUsers(this ModelBuilder modelBuilder)
    {
        var email = "JvF1w@example.com";
        var password = "123456";
        var isVerified = true;

        var isCreatedAccount = true;
        var firstName = "Vova";
        var lastName = "Pupkin";
        var isATeacher = true;
        var isAnExpert = false;
        var countryTitle = "Ukraine";
        var cityTitle = "Kyiv";
        var institutionTitles = "University of Kyiv";
        var disciplineTitles = new List<string> { "Math"};
        var languageTitles = new List<string> { "English" };
        var gradeNumbers = new List<int> { 8};
        var imageUrl = "https://s3.eu-north-1.amazonaws.com/hiclass.images/default/Female-Teacher.png";

        var countryId = Guid.NewGuid();
        modelBuilder.Entity<Country>()
            .HasData(new Country()
            {
                CountryId = countryId,
                Title = countryTitle
            });

        var cityId = Guid.NewGuid();
        modelBuilder.Entity<City>()
            .HasData(new City()
            {
                CityId = cityId,
                Title = cityTitle,
            });

        var institutionId = Guid.NewGuid();
        modelBuilder.Entity<Institution>()
            .HasData(new Institution()
            {
                InstitutionId = institutionId,
                Title = institutionTitles
            });

        var disciplineIds = disciplineTitles
            .Select(title => DictionaryDisplineIds.FirstOrDefault(x => 
                x.Key == title).Value)
            .ToList();

        var languageIds = languageTitles
            .Select(title => DictionaryLanguageIds.FirstOrDefault(x => 
                x.Key == title).Value)
            .ToList();
        
        var gradeIds = gradeNumbers
            .Select(number => DictionaryGradeIds.FirstOrDefault(x => 
                x.Key == number.ToString()).Value)
            .ToList();

        var userId = Guid.NewGuid();
        var user = new User()
        {
            UserId = userId,
            Email = email,
            IsVerified = isVerified,
            IsCreatedAccount = isCreatedAccount,
            FirstName = firstName,
            LastName = lastName,
            IsATeacher = isATeacher,
            IsAnExpert = isAnExpert,
            CountryId = countryId,
            CityId = cityId,
            InstitutionId = institutionId,
            ImageUrl = imageUrl
        };
        
        modelBuilder.Entity<User>().HasData(user);

        modelBuilder.Entity<UserDiscipline>()
            .HasData(disciplineIds.Select(disciplineId =>
                new UserDiscipline()
                {
                    UserId = userId,
                    DisciplineId = disciplineId
                }));

        modelBuilder.Entity<UserLanguage>()
            .HasData(languageIds.Select(languageId =>
                new UserLanguage()
                {
                    UserId = userId,
                    LanguageId =languageId
                }));
        
        modelBuilder.Entity<UserGrade>()
            .HasData(gradeIds.Select(gradeId =>
                new UserGrade()
                {
                    UserId = userId,
                    GradeId = gradeId
                }));

    }
}