using HiClass.Application.Handlers.EntityHandlers.ClassHandlers.Commands.DeleteClass;
using HiClass.Application.Tests.Common;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HiClass.Application.Tests.Handlers.EntityHandlers.ClassHandlers.Commands;

public class DeleteClassCommandHandlerTests : TestCommonBase
{
    [Fact]
    public async Task DeleteClassCommand_Handle_ShouldDeleteClass()
    {
        // Arrange
        var userOwnerId = SharedLessonDbContextFactory.UserAId;
        var classForDeleteId = SharedLessonDbContextFactory.ClassForDeleteId;
        var handler = new DeleteClassCommandHandler(Context);

        // Act
        await handler.Handle(new DeleteClassCommand()
            {
                ClassId = classForDeleteId,
                UserOwnerId = userOwnerId
            }
            , CancellationToken.None);

        // Assert
        Assert.Null(await Context.Classes.SingleOrDefaultAsync(c =>
            c.ClassId == SharedLessonDbContextFactory.ClassForDeleteId));
    }
}