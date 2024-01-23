using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Handlers.EntityConnectionHandlers.UserGradeHandlers.Commands.DeleteUserGrade;
using HiClass.Application.Tests.Common;
using Xunit;

namespace HiClass.Application.Tests.Handlers.EntityConnectionHandlers.UserGradeHandlers.Commands;

public class DeleteUserGradeCommandHandlerTests : TestCommonBase
{
    [Fact]
    public async Task DeleteUserGradeCommandHandler_Handle_ShouldDeleteUserGrade()
    {
        // Arrange
        var userId = SharedLessonDbContextFactory.UserAId;
        var user = Context.Users.SingleOrDefault(x => 
            x.UserId == userId);
        var gradeForDelete = Context.Grades
            .SingleOrDefault(x => x.GradeNumber == 10)!;

        var handler = new DeleteUserGradeCommandHandler(Context);

        // Act
        await handler.Handle(new DeleteUserGradeCommand
        {
            UserId = userId,
            GradeId = gradeForDelete.GradeId
        }, CancellationToken.None);

        // Assert
        Assert.Empty(Context.UserGrades
            .Where(ug => ug.UserId == userId &&
                         ug.Grade.GradeId == gradeForDelete.GradeId));
    }

    [Fact]
    public async Task DeleteUserGradeCommandHandler_Handle_FailOnWrongId()
    {
        // Arrange
        var user = Context.Users.SingleOrDefault(x =>
            x.UserId == SharedLessonDbContextFactory.UserRegisteredId);
        var gradeForDelete = Context.Grades.SingleOrDefault(x => x.GradeNumber == 4)!;

        var handler = new DeleteUserGradeCommandHandler(Context);

        // Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await handler.Handle(new DeleteUserGradeCommand
            {
                UserId = user.UserId,
                GradeId = gradeForDelete.GradeId
            }, CancellationToken.None));
    }
}