using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetUserById;
using HiClass.Application.Tests.Common;
using Xunit;

namespace HiClass.Application.Tests.Handlers.EntityHandlers.UserHandlers.Queries;

public class GetUserCommandHandlerTests : TestCommonBase
{
    [Fact]
    public void Handle_ShouldReturnUser()
    {
        // Arrange
        var userId = SharedLessonDbContextFactory.UserAId;
        var handler = new GetUserByIdQueryHandler(Context);

        // Act
        var result = handler.Handle(new GetUserByIdQuery(userId), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var handler = new GetUserByIdQueryHandler(Context);

        // Act
        // Assert
        await Assert.ThrowsAsync<UserNotFoundByIdException>(() =>
            handler.Handle(new GetUserByIdQuery(userId), CancellationToken.None));
    }
}