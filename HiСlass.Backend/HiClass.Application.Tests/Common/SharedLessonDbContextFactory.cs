using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Communication;
using HiClass.Domain.Entities.Job;
using HiClass.Domain.Entities.Location;
using HiClass.Domain.Entities.Main;
using HiClass.Domain.EntityConnections;
using HiClass.Domain.Enums;
using HiClass.Domain.Enums.EntityTypes;
using HiClass.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HiClass.Application.Tests.Common;

public class SharedLessonDbContextFactory
{
    public static readonly Guid UserAId = Guid.NewGuid();
    public static readonly Guid UserBId = Guid.NewGuid();
    public static readonly Guid UserRegisteredId = Guid.NewGuid();
    public static readonly Guid UserForDeleteId = Guid.NewGuid();

    public static readonly Guid CountryAId = Guid.NewGuid();
    public static readonly Guid CountryBId = Guid.NewGuid();
    public static readonly Guid CountryForDeleteId = Guid.NewGuid();

    public static readonly Guid CityAId = Guid.NewGuid();
    public static readonly Guid CityBId = Guid.NewGuid();
    public static readonly Guid CityForDeleteId = Guid.NewGuid();

    public static readonly Guid EstablishmentAId = Guid.NewGuid();
    public static readonly Guid EstablishmentBId = Guid.NewGuid();
    public static readonly Guid EstablishmentForDeleteId = Guid.NewGuid();

    public static readonly Guid ClassAId = Guid.NewGuid();
    public static readonly Guid ClassBId = Guid.NewGuid();
    public static readonly Guid ClassForUpdateId = Guid.NewGuid();
    public static readonly Guid ClassForDeleteId = Guid.NewGuid();

    public static readonly Guid InvitationAId = Guid.NewGuid();
    public static readonly Guid InvitationBId = Guid.NewGuid();
    public static readonly Guid InvitationForDeleteId = Guid.NewGuid();

    public static readonly Guid FeedbackAId = Guid.NewGuid();
    public static readonly Guid FeedbackBId = Guid.NewGuid();
    public static readonly Guid FeedbackForDeleteId = Guid.NewGuid();

    private static IConfiguration? _configuration;

    public SharedLessonDbContextFactory(IConfiguration? configuration)
    {
        _configuration = configuration;
    }

    public static SharedLessonDbContext Create()
    {
        var options = new DbContextOptionsBuilder<SharedLessonDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        var context = new SharedLessonDbContext(options, _configuration);
        context.Database.EnsureCreated();

        AppendCountries(context);
        AppendCities(context);
        AppendEstablishments(context);

        context.SaveChangesAsync();
        AppendUsers(context);

        AppendClasses(context);
        context.SaveChangesAsync();

        AppendUserDisciplines(context);
        AppendUserLanguages(context);
        AppendUserGrades(context);
        AppendUserRoles(context);
        AppendClassLanguages(context);
        AppendEstablishmentTypesEstablishment(context);

        AppendInvitations(context);
        context.SaveChangesAsync();

        AppendFeedbacks(context);
        context.SaveChangesAsync();
        return context;
    }

    private static void AppendUsers(ISharedLessonDbContext context)
    {
        context.Users.AddRange(
            new User()
            {
                UserId = UserRegisteredId,
                Email = "emailR",
                IsCreatedAccount = false
            },
            new User
            {
                UserId = UserAId,
                Email = "emailA",
                IsCreatedAccount = true,
                FirstName = "FirstNameA",
                LastName = "LastNameA",
                IsATeacher = true,
                IsAnExpert = false,
                Institution = context.Institutions.FirstOrDefault(e =>
                    e.InstitutionId == EstablishmentAId),
                City = context.Cities.FirstOrDefault(c =>
                    c.CityId == CityAId),
                Country = context.Countries.FirstOrDefault(c =>
                    c.CountryId == CountryAId),
                Description = "DescriptionA",
                ImageUrl = "PhotoUrlA",
                BannerImageUrl = "BannerPhotoUrlA",
                RegisteredAt = DateTime.Today,
                CreatedAt = DateTime.Today,
                IsVerified = true,
                VerifiedAt = default,
                LastOnlineAt = DateTime.Today,
                DeletedAt = default,
            },
            new User
            {
                UserId = UserBId,
                Email = "emailB",
                // Password = "passwordB",
                FirstName = "FirstNameB",
                LastName = "LastNameB",
                IsCreatedAccount = true,
                IsATeacher = true,
                IsAnExpert = true,
                Institution = context.Institutions.FirstOrDefault(e =>
                    e.InstitutionId == EstablishmentAId)!,
                City = context.Cities.FirstOrDefault(c =>
                    c.CityId == CityBId)!,
                Country = context.Countries.FirstOrDefault(c =>
                    c.CountryId == CountryBId)!,
                Description = "DescriptionB",
                ImageUrl = "PhotoUrlB",
                BannerImageUrl = "BannerPhotoUrlB",
                RegisteredAt = DateTime.Today,
                CreatedAt = DateTime.Today,
                IsVerified = true,
                VerifiedAt = default,
                LastOnlineAt = DateTime.Today,
                DeletedAt = default,
            },
            new User()
            {
                UserId = UserForDeleteId,
                FirstName = "FirstNameForDelete",
                LastName = "LastNameForDelete",
                Email = "emailForDelete",
                // Password = "passwordForDelete",
                IsVerified = true
            });
    }

    private static void AppendClasses(SharedLessonDbContext context)
    {
        context.Classes.AddRange(
            new Class()
            {
                UserId = UserAId,
                ClassId = ClassAId,
                Title = "ClassAId",
                Grade = context.Grades.FirstOrDefault(g =>
                    g.GradeNumber == 10)!,
                ImageUrl = "PhotoUrl",
                CreatedAt = DateTime.Today
            },
            new Class()
            {
                UserId = UserAId,
                ClassId = ClassForUpdateId,
                Title = "ClassForUpdateId",
                Grade = context.Grades.FirstOrDefault(g => g.GradeNumber == 5)!,
                ImageUrl = "PhotoUrl",
                CreatedAt = DateTime.Today
            },
            new Class()
            {
                UserId = UserAId,
                ClassId = ClassForDeleteId,
                Title = "ClassForDeleteId",
                Grade = context.Grades.FirstOrDefault(g => g.GradeNumber == 5)!,
                ImageUrl = "PhotoUrl",
                CreatedAt = DateTime.Today
            },
            new Class()
            {
                UserId = UserBId,
                ClassId = ClassBId,
                Title = "titleB",
                Grade = context.Grades.FirstOrDefault(g => g.GradeNumber == 6)!,
                ImageUrl = "PhotoUrl",
                CreatedAt = DateTime.Today
            }
        );
    }

    private static void AppendInvitations(ISharedLessonDbContext context)
    {
        context.Invitations.AddRange(
            new Invitation()
            {
                InvitationId = InvitationAId,
                UserSenderId = UserBId,
                UserRecipientId = UserAId,
                ClassSenderId = ClassBId,
                ClassRecipientId = ClassAId,
                DateOfInvitation = DateTime.Today,
                Status = InvitationStatus.Pending
            },
            new Invitation()
            {
                InvitationId = InvitationBId,
                UserSenderId = UserBId,
                UserRecipientId = UserAId!,
                ClassSenderId = ClassBId,
                ClassRecipientId = ClassAId!,
                DateOfInvitation = DateTime.Today,
                Status = InvitationStatus.Pending
            },
            new Invitation()
            {
                InvitationId = InvitationForDeleteId,
                UserSender = context.Users.FirstOrDefault(u =>
                    u.UserId == UserAId)!,
                UserRecipient = context.Users.FirstOrDefault(u =>
                    u.UserId == UserBId)!,
                ClassSender = context.Classes.FirstOrDefault(c =>
                    c.ClassId == ClassAId)!,
                ClassRecipient = context.Classes.FirstOrDefault(c =>
                    c.ClassId == ClassBId)!,
                DateOfInvitation = DateTime.Today,
                Status = InvitationStatus.Pending
            }
        );
    }

    private static void AppendFeedbacks(ISharedLessonDbContext context)
    {
        context.Feedbacks.AddRangeAsync(
            new Feedback()
            {
                FeedbackId = FeedbackAId,
                Invitation = context.Invitations.FirstOrDefault(i
                    => i.InvitationId == InvitationAId)!,
                UserFeedbackReceiver = context.Users.FirstOrDefault(u =>
                    u.UserId == UserAId)!,
                UserFeedbackSender = context.Users.FirstOrDefault(u =>
                    u.UserId == UserBId)!,
                WasTheJointLesson = true,
                ReasonForNotConducting = null,
                FeedbackText = "ABOBA",
                Rating = 5
            },
            new Feedback()
            {
                FeedbackId = FeedbackBId,
                Invitation = context.Invitations.FirstOrDefault(i
                    => i.InvitationId == InvitationBId)!,
                UserFeedbackReceiver = context.Users.FirstOrDefault(u =>
                    u.UserId == UserBId)!,
                UserFeedbackSender = context.Users.FirstOrDefault(u =>
                    u.UserId == UserAId)!,
                WasTheJointLesson = true,
                ReasonForNotConducting = null,
                FeedbackText = "ABOBA",
                Rating = 5
            },
            new Feedback()
            {
                FeedbackId = FeedbackForDeleteId,
                Invitation = context.Invitations.FirstOrDefault(i
                    => i.InvitationId == InvitationForDeleteId)!,
                WasTheJointLesson = true,
                ReasonForNotConducting = null,
                FeedbackText = "ABOBA",
                Rating = 5
            }
        );
    }

    private static void AppendUserRoles(ISharedLessonDbContext context)
    {
        context.UserRoles.AddRangeAsync(
            new UserRole()
            {
                Role = context.Roles.FirstOrDefault(r => r.Title == "User")!,
                User = context.Users.FirstOrDefault(u => u.UserId == UserAId)!
            },
            new UserRole()
            {
                Role = context.Roles.FirstOrDefault(r => r.Title == "User")!,
                User = context.Users.FirstOrDefault(u => u.UserId == UserBId)!
            }
        );
    }

    private static void AppendClassLanguages(ISharedLessonDbContext context)
    {
        context.ClassLanguages.AddRangeAsync(
            new ClassLanguage()
            {
                Class = context.Classes.FirstOrDefault(c => c.ClassId == ClassAId)!,
                Language = context.Languages.FirstOrDefault(l => l.Title == "English")!
            },
            new ClassLanguage()
            {
                Class = context.Classes.FirstOrDefault(c => c.ClassId == ClassForUpdateId)!,
                Language = context.Languages.FirstOrDefault(l => l.Title == "English")!
            },
            new ClassLanguage()
            {
                Class = context.Classes.FirstOrDefault(c => c.ClassId == ClassForDeleteId)!,
                Language = context.Languages.FirstOrDefault(l => l.Title == "English")!
            }
        );
    }

    private static void AppendUserGrades(ISharedLessonDbContext context)
    {
        context.UserGrades.AddRange(
            new UserGrade()
            {
                User = context.Users.SingleAsync(u => u.UserId == UserAId).Result,
                Grade = context.Grades.SingleAsync(g => g.GradeNumber == 10).Result
            },
            new UserGrade()
            {
                User = context.Users.SingleAsync(u => u.UserId == UserAId).Result,
                Grade = context.Grades.SingleAsync(g => g.GradeNumber == 6).Result
            });
    }

    private static void AppendUserDisciplines(ISharedLessonDbContext context)
    {
        context.UserDisciplines.AddRange(
            new UserDiscipline()
            {
                User = context.Users.FirstOrDefault(u =>
                    u.UserId == UserAId)!,
                Discipline = context.Disciplines.FirstOrDefault(d =>
                    d.Title == "Chemistry")!,
            },
            new UserDiscipline
            {
                User = context.Users.SingleAsync(u =>
                    u.UserId == UserAId).Result,
                Discipline = context.Disciplines.SingleAsync(d =>
                    d.Title == "Biology").Result
            });
    }

    private static void AppendUserLanguages(ISharedLessonDbContext context)
    {
        context.UserLanguages.AddRange(
            new UserLanguage()
            {
                User = context.Users.FirstOrDefault(u =>
                    u.UserId == UserAId)!,
                Language = context.Languages.FirstOrDefault(l =>
                    l.Title == "English")!
            },
            new UserLanguage()
            {
                User = context.Users.SingleAsync(u =>
                    u.UserId == UserAId).Result,
                Language = context.Languages.SingleAsync(l =>
                    l.Title == "Russian").Result
            },
            new UserLanguage()
            {
                User = context.Users.SingleAsync(u =>
                    u.UserId == UserAId).Result,
                Language = context.Languages.SingleAsync(l =>
                    l.Title == "Italian").Result
            }
        );
    }

    private static void AppendCountries(ISharedLessonDbContext context)
    {
        context.Countries.AddRange(
            new Country()
            {
                CountryId = CountryAId,
                Title = "CountryA"
            },
            new Country()
            {
                CountryId = CountryBId,
                Title = "CountryB"
            },
            new Country
            {
                CountryId = CountryForDeleteId,
                Title = "CountryForDelete"
            });
    }

    private static void AppendCities(ISharedLessonDbContext context)
    {
        context.Cities.AddRange(
            new City()
            {
                CityId = CityAId,
                CountryId = CountryAId,
                Title = "CityA"
            },
            new City()
            {
                CityId = CityBId,
                CountryId = CountryBId,
                Title = "CityB"
            },
            new City
            {
                CityId = CityForDeleteId,
                CountryId = CountryBId,
                Title = "CityForDelete"
            });
    }

    private static void AppendEstablishments(ISharedLessonDbContext context)
    {
        context.Institutions.AddRange(
            new Institution()
            {
                InstitutionId = EstablishmentAId,
                Title = "EstablishmentA",
                Address = "AddressA",
            },
            new Institution()
            {
                InstitutionId = EstablishmentBId,
                Title = "EstablishmentB",
                Address = "AddressB",
            },
            new Institution()
            {
                InstitutionId = EstablishmentForDeleteId,
                Title = "EstablishmentForDelete",
                Address = "AddressC",
            });
    }

    private static void AppendEstablishmentTypesEstablishment(ISharedLessonDbContext context)
    {
        context.InstitutionTypesInstitutions.AddRange(
            new InstitutionTypeInstitution()
            {
                InstitutionTypeId = context.InstitutionTypes.SingleAsync(
                        et
                            => et.Title == "School")
                    .Result.InstitutionTypeId,

                InstitutionId = EstablishmentAId
            },
            new InstitutionTypeInstitution
            {
                InstitutionTypeId = context.InstitutionTypes.SingleAsync(et
                        => et.Title == "Gymnasium")
                    .Result.InstitutionTypeId,
                InstitutionId = EstablishmentAId
            },
            new InstitutionTypeInstitution
            {
                InstitutionTypeId = context.InstitutionTypes.SingleAsync(
                        et
                            => et.Title == "School")
                    .Result.InstitutionTypeId,

                InstitutionId = EstablishmentBId
            }
        );
    }


    public static void Destroy(SharedLessonDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Dispose();
    }
}