using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Handlers.EntityConnectionHandlers.UserDisciplinesHandlers.Commands.DeleteUserDisciplines;
using HiClass.Application.Tests.Common;
using Xunit;

namespace HiClass.Application.Tests.Handlers.EntityConnectionHandlers.UserDisciplineHandlers.Commands;

public class DeleteUserDisciplineCommandHandlerTests : TestCommonBase
{
    [Fact]
    public async Task DeleteUserDisciplineCommandHandler_Handle_ShouldDeleteUser()
    {
        // Arrange
        var userForDeleteDisciplineId = SharedLessonDbContextFactory.UserAId;
        var user = Context.Users.SingleOrDefault(x => x.UserId == userForDeleteDisciplineId);
        var disciplineForDelete = Context.Disciplines.SingleOrDefault(x => x.Title == "Chemistry")!;


        var handler = new DeleteUserDisciplineCommandHandler(Context);

        // Act
        await handler.Handle(new DeleteUserDisciplineCommand
        {
            User = user!,
            Discipline = disciplineForDelete
        }, CancellationToken.None);

        // Assert
        Assert.Empty(Context.UserDisciplines
            .Where(ud => ud.UserId == userForDeleteDisciplineId &&
                         ud.Discipline.Title == disciplineForDelete.Title));
    }

    [Fact]
    public async Task DeleteUserDisciplineCommandHandler_Handle_FailOnWrongUserId()
    {
        // Arrange
        var user = Context.Users.SingleOrDefault(x =>
            x.UserId == SharedLessonDbContextFactory.UserRegisteredId);
        var disciplineForDelete = Context.Disciplines
            .SingleOrDefault(x => x.Title == "Chemistry")!;

        var handler = new DeleteUserDisciplineCommandHandler(Context);

        // Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await handler.Handle(new DeleteUserDisciplineCommand
            {
                User = user!,
                Discipline = disciplineForDelete
            }, CancellationToken.None));
    }
}