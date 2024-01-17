using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.DeleteUser;
using HiClass.Application.Tests.Common;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HiClass.Application.Tests.Handlers.EntityHandlers.UserHandlers.Commands;

public class DeleteUserCommandHandlerTests : TestCommonBase
{
    [Fact]
    public async Task DeleteUserCommandHandler_Handle_ShouldDeleteUser()
    {
        // Arrange
        var userForDelete = await Context.Users.SingleOrDefaultAsync(u =>
            u.UserId == SharedLessonDbContextFactory.UserForDeleteId);

        var handler = new DeleteUserCommandHandler(Context);

        // Act
        await handler.Handle(new DeleteUserCommand(userForDelete!.UserId), CancellationToken.None);

        // Assert
        Assert.Null(await Context.Users.SingleOrDefaultAsync(u =>
            u.UserId == userForDelete!.UserId));
    }
}