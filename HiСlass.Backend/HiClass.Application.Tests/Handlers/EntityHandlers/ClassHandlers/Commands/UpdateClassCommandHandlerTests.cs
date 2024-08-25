using HiClass.Application.Handlers.EntityHandlers.ClassHandlers.Commands.EditClass;
using HiClass.Application.Tests.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace HiClass.Application.Tests.Handlers.EntityHandlers.ClassHandlers.Commands;

public class UpdateClassCommandHandlerTests : TestCommonBase
{
    [Fact]
    public async Task UpdateClassCommand_Handle_ShouldUpdateClass()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var @class = await Context.Classes.FirstAsync(c =>
            c.ClassId == SharedLessonDbContextFactory.ClassForUpdateId);
        const string newTitle = "New Title";
        const string newPhotoUrl = "New PhotoUrl";
        var newDiscipline = "Mathematics";
        var newLanguages = new List<string>
        {
            "Russian"
        };

        var newGrade = await Context.Grades.FirstAsync(g => g.GradeNumber == 10);

        var handler = new EditClassCommandHandler(Context, mediatorMock.Object);

        // Act
        await handler.Handle(new EditClassCommand()
        {
            ClassId = @class.ClassId,
            Title = newTitle,
            DisciplineTitle = newDiscipline,
            LanguageTitles = newLanguages,
            GradeNumber = newGrade.GradeNumber,
        }, CancellationToken.None);

        // Assert
        var result = await Context.Classes.FirstOrDefaultAsync(c =>
            c.ClassId == @class.ClassId &&
            c.Title == newTitle &&
            c.Grade == newGrade &&
            c.ClassLanguages.Count == newLanguages.Count);

        Assert.NotNull(result);
        Assert.True(result != null
                    && result.ClassLanguages.Select(l =>
                            l.Language)
                        .Select(l => l.Title)
                        .SequenceEqual(newLanguages));

    }
}