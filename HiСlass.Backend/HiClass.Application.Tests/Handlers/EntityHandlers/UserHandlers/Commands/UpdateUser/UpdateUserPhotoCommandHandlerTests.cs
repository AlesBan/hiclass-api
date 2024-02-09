using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserPhoto;
using HiClass.Application.Tests.Common;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HiClass.Application.Tests.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUser;

public class UpdateUserPhotoCommandHandlerTests : TestCommonBase
{
    [Fact]
    public async Task UpdateUserPhotoCommandHandler_Handle_ShouldUpdateUserPhoto()
    {
        // Arrange
        var userId = SharedLessonDbContextFactory.UserAId;
        const string newPhotoUrl = "NewPhotoUrl";

        var handler = new UpdateUserImageCommandHandler(Context);

        // Act
        await handler.Handle(new UpdateUserImageCommand(userId, newPhotoUrl), CancellationToken.None);

        // Assert
        Assert.NotNull(await Context.Users.SingleOrDefaultAsync(u =>
            u.UserId == userId &&
            u.ImageUrl == newPhotoUrl));
    }
}