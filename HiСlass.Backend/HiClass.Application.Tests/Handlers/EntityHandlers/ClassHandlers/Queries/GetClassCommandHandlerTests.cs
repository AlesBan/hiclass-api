using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Handlers.EntityHandlers.ClassHandlers.Queries.GetClass;
using HiClass.Application.Tests.Common;
using HiClass.Domain.Entities.Main;
using Shouldly;
using Xunit;

namespace HiClass.Application.Tests.Handlers.EntityHandlers.ClassHandlers.Queries;

public class GetClassCommandHandlerTests : TestCommonBase
{
    [Fact]
    public async Task Handle_ShouldReturnClass()
    {
        // Arrange
        var classId = SharedLessonDbContextFactory.ClassAId;
        var handler = new GetClassByIdQueryHandler(Context);

        // Act
        var result = await handler.Handle(new GetClassByIdQuery(classId), CancellationToken.None);

        // Assert
        result.ShouldBeOfType<Class>();
        result.ClassId.ShouldBe(classId);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException()
    {
        // Arrange
        var classId = Guid.NewGuid();
        var handler = new GetClassByIdQueryHandler(Context);

        // Act
        await Should.ThrowAsync<NotFoundException>(async () =>
            await handler.Handle(new GetClassByIdQuery(classId), CancellationToken.None));
    }
}