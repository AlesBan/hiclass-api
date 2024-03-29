using HiClass.Application.Handlers.EntityConnectionHandlers.UserDisciplinesHandlers.Commands.CreateUserDisciplines;
using HiClass.Application.Tests.Common;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HiClass.Application.Tests.Handlers.EntityConnectionHandlers.UserDisciplineHandlers.Commands;

public class CreateUserDisciplineCommandHandlerTests : TestCommonBase
{
    [Fact]
    public async Task CreateUserDisciplinesCommandHandler_Handle_ShouldCreateUserDiscipline()
    {
        // Arrange
        var user = await Context.Users
            .SingleOrDefaultAsync(x =>
                x.UserId == SharedLessonDbContextFactory.UserRegisteredId);
        var discipline = Extensions.PickRandom(Context.Disciplines.ToList(), 1).ToList().First();

        var handler = new CreateUserDisciplinesCommandHandler(Context);

        // Act
        await handler.Handle(new CreateUserDisciplinesCommand()
        {
            UserId = user!.UserId,
            DisciplineIds = new List<Guid> { discipline.DisciplineId }
        }, CancellationToken.None);

        // Assert
        Assert.NotEmpty(Context.UserDisciplines
            .Where(ud => ud.UserId == user!.UserId &&
                         ud.Discipline.Title == discipline.Title));
    }
}