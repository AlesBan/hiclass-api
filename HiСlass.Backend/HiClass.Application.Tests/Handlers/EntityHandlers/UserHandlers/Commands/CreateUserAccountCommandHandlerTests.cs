// using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.CreateUserAccount;
// using HiClass.Application.Helpers.TokenHelper;
// using HiClass.Application.Tests.Common;
// using Microsoft.EntityFrameworkCore;
// using Moq;
// using Xunit;
//
// namespace HiClass.Application.Tests.Handlers.EntityHandlers.UserHandlers.Commands;
//
// public class CreateUserAccountCommandHandlerTests : TestCommonBase
// {
//     [Fact]
//     public async Task CreateUserAccountCommand_Handle_ShouldCreateUserAccount()
//     {
//         // Arrange
//         var user = await Context.Users.SingleOrDefaultAsync(u =>
//             u.UserId == SharedLessonDbContextFactory.UserRegisteredId);
//         const string firstName = "FirstName";
//         const string lastName = "LastName";
//         const bool isATeacher = true;
//         const bool isAnExpert = false;
//         var cityLocation = Context.Cities.FirstOrDefault(c =>
//             c.CityId == SharedLessonDbContextFactory.CityAId);
//
//         var institution = Context.Institutions.FirstOrDefault(e =>
//             e.InstitutionId == SharedLessonDbContextFactory.EstablishmentAId);
//         var disciplines = Extensions.PickRandom(Context.Disciplines.ToList(), 3).ToList();
//         var languages = Extensions.PickRandom(Context.Languages.ToList(), 2).ToList();
//
//         var createUserAccountCommand = new CreateUserAccountCommand()
//         {
//             UserId = user!.UserId,
//             FirstName = firstName,
//             LastName = lastName,
//             IsATeacher = isATeacher,
//             IsAnExpert = isAnExpert,
//             CityId = cityLocation!.CityId,
//             InstitutionId = institution!.InstitutionId,
//             DisciplineIds = disciplines.Select(d => d.DisciplineId).ToList(),
//             LanguageIds = languages.Select(l => l.LanguageId).ToList(),
//         };
//
//         var mockTokenHelper = new Mock<ITokenHelper>();
//
//
//         // Act
//         var handler = new CreateUserAccountCommandHandler(Context);
//         await handler.Handle(createUserAccountCommand,
//             CancellationToken.None);
//
//         var result = await Context.Users.SingleOrDefaultAsync(u =>
//             u.UserId == user.UserId &&
//             u.FirstName == firstName &&
//             u.LastName == lastName &&
//             u.IsATeacher == isATeacher &&
//             u.IsAnExpert == isAnExpert &&
//             u.City!.CityId == cityLocation.CityId &&
//             u.Institution!.InstitutionId == institution.InstitutionId &&
//             u.UserDisciplines.Count() == disciplines.Count() &&
//             u.UserLanguages.Count() == languages.Count());
//         // Assert
//         Assert.NotNull(result);
//     }
// }