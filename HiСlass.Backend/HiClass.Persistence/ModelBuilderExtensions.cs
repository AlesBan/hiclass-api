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
    private static Dictionary<string, Guid> DictionaryDisciplineIds { get; set; } = new();
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

        modelBuilder.SeedMultipleUsers();
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

        DictionaryGradeIds.Clear();
        foreach (var grade in grades)
        {
            DictionaryGradeIds.Add(grade.GradeNumber.ToString(), grade.GradeId);
        }

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

        DictionaryDisciplineIds.Clear();
        foreach (var discipline in disciplineList)
        {
            DictionaryDisciplineIds.Add(discipline.Title, discipline.DisciplineId);
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

        DictionaryLanguageIds.Clear();
        foreach (var language in languageList)
        {
            DictionaryLanguageIds.Add(language.Title, language.LanguageId);
        }

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

    private static void SeedMultipleUsers(this ModelBuilder modelBuilder)
    {
        var random = new Random();

        var belarusId = Guid.NewGuid();
        var minskId = Guid.NewGuid();
        var russiaId = Guid.NewGuid();
        var moscowId = Guid.NewGuid();

        var belarus = new Country
        {
            CountryId = belarusId,
            Title = "Belarus"
        };
        var minsk = new City
        {
            CityId = minskId,
            Title = "Minsk",
            CountryId = belarusId
        };

        var russia = new Country
        {
            CountryId = russiaId,
            Title = "Russia"
        };
        var moscow = new City
        {
            CityId = moscowId,
            Title = "Moscow",
            CountryId = russiaId
        };

        modelBuilder.SeedEntity(belarus);
        modelBuilder.SeedEntity(minsk);
        modelBuilder.SeedEntity(russia);
        modelBuilder.SeedEntity(moscow);

        for (int i = 0; i < 40; i++)
        {
            var email = $"user{i + 1}@example.com";
            var password = "111111";
            var isVerified = true;
            var isCreatedAccount = true;
            var firstName = GenerateRandomString(random, 6);
            var lastName = GenerateRandomString(random, 8);
            var isATeacher = true;
            var isAnExpert = false;
            var countryId = random.Next(0, 2) == 0 ? belarusId : russiaId;
            var cityId = countryId == belarusId ? minskId : moscowId;
            var institutionTitles = GenerateRandomString(random, 14);
            var disciplineTitle = GetRandomKey(DictionaryDisciplineIds, random);
            var languageTitle = GetRandomKey(DictionaryLanguageIds, random);
            var gradeTitle = GetRandomKey(DictionaryGradeIds, random);
            var userImageUrl = "https://s3.eu-north-1.amazonaws.com/hiclass.images/default/Female-Teacher.png";

            var institutionId = Guid.NewGuid();
            var institution = new Institution
            {
                InstitutionId = institutionId,
                Title = institutionTitles,
                Address = countryId == belarusId ? "Belarus, Minsk" : "Russia, Moscow"
            };

            var disciplineId = DictionaryDisciplineIds[disciplineTitle];
            var languageId = DictionaryLanguageIds[languageTitle];
            var gradeId = DictionaryGradeIds[gradeTitle];

            var userId = Guid.NewGuid();
            var user = new User
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
                ImageUrl = userImageUrl
            };

            PasswordHelper.SetUserPasswordHash(user, password);

            modelBuilder.SeedEntity(institution);
            modelBuilder.SeedEntity(user);
            modelBuilder.SeedUserDisciplines(userId, disciplineId);
            modelBuilder.SeedUserLanguages(userId, languageId);
            modelBuilder.SeedUserGrades(userId, gradeId);

            var newClass = CreateClass(userId, user.FirstName, gradeId, disciplineId, languageId);
            modelBuilder.SeedEntity(newClass.Class);
            modelBuilder.SeedEntity(newClass.ClassDiscipline);
            modelBuilder.SeedEntity(newClass.ClassLanguage);
        }
    }

    private static string GenerateRandomString(Random random, int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private static string GetRandomKey(Dictionary<string, Guid> dictionary, Random random)
    {
        var index = random.Next(dictionary.Count);
        return dictionary.ElementAt(index).Key;
    }

    private static (Class Class, ClassDiscipline ClassDiscipline, ClassLanguage ClassLanguage) CreateClass(Guid userId,
        string userName,
        Guid gradeId, Guid disciplineId, Guid languageId)
    {
        var classId = Guid.NewGuid();
        var classImageUrl = "https://s3.eu-north-1.amazonaws.com/hiclass.images/default/default-class.jpg";
        var newClass = new Class
        {
            ClassId = classId,
            Title = $"Class of {userName}",
            UserId = userId,
            ImageUrl = classImageUrl,
            GradeId = gradeId,
        };

        var classDiscipline = new ClassDiscipline
        {
            ClassId = classId,
            DisciplineId = disciplineId
        };

        var classLanguage = new ClassLanguage
        {
            ClassId = classId,
            LanguageId = languageId
        };

        return (newClass, classDiscipline, classLanguage);
    }

    private static void SeedEntity<TEntity>(this ModelBuilder modelBuilder, TEntity entity) where TEntity : class
    {
        modelBuilder.Entity<TEntity>().HasData(entity);
    }

    private static void SeedUserDisciplines(this ModelBuilder modelBuilder, Guid userId, Guid disciplineId)
    {
        var userDiscipline = new UserDiscipline
        {
            UserId = userId,
            DisciplineId = disciplineId
        };

        modelBuilder.Entity<UserDiscipline>().HasData(userDiscipline);
    }

    private static void SeedUserLanguages(this ModelBuilder modelBuilder, Guid userId, Guid languageId)
    {
        var userLanguage = new UserLanguage
        {
            UserId = userId,
            LanguageId = languageId
        };
        modelBuilder.Entity<UserLanguage>().HasData(userLanguage);
    }

    private static void SeedUserGrades(this ModelBuilder modelBuilder, Guid userId, Guid gradeId)
    {
        var userGrade = new UserGrade
        {
            UserId = userId,
            GradeId = gradeId
        };
        modelBuilder.Entity<UserGrade>().HasData(userGrade);
    }
}