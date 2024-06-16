// using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.EditUserEmail;
// using HiClass.Application.Tests.Common;
// using Microsoft.EntityFrameworkCore;
// using Xunit;
//
// namespace HiClass.Application.Tests.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUser;
//
// public class UpdateUserEmailCommandHandlerTests : TestCommonBase
// {
//     [Fact]
//     public async Task UpdateUserEmailCommandHandler_Handle_ShouldUpdateUserEmail()
//     {
//         // Arrange
//         var userId = SharedLessonDbContextFactory.UserAId;
//         const string newEmail = "NewEmail";
//
//         var handler = new EditUserEmailAndRemoveVerificationCommandHandler(Context);
//
//         // Act
//         await handler.Handle(new EditUserEmailAndRemoveVerificationCommand()
//         {
//             UserId = userId,
//             Email = newEmail
//         }, CancellationToken.None);
//
//         // Assert
//         Assert.NotNull(await Context.Users.SingleOrDefaultAsync(u =>
//             u.UserId == userId &&
//             u.Email == newEmail));
//     }
// }