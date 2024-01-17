using AutoMapper;
using HiClass.Application.Handlers.EntityConnectionHandlers.UserGradeHandlers.Commands.UpdateUserGrades;
using HiClass.Application.Tests.Common;
using HiClass.Domain.Entities.Education;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace HiClass.Application.Tests.Handlers.EntityConnectionHandlers.UserGradeHandlers.Commands;

public class UpdateUserGradesCommandHandlerTests : TestCommonBase
{
    [Fact]
    public async Task UpdateUserGradesCommandHandler_Handle_ShouldUpdateUserGrades()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var user = await Context.Users
            .SingleOrDefaultAsync(x =>
                x.UserId == SharedLessonDbContextFactory.UserAId);
        var newGrades = new List<Grade>()
        {
            Context.Grades.SingleAsync(x =>
                x.GradeNumber == 8).Result
        };
        var handler = new UpdateUserGradesCommandHandler(Context, mediatorMock.Object);

        // Act
        await handler.Handle(new UpdateUserGradesCommand
        {
            UserId = user!.UserId,
            NewGradeIds = newGrades.Select(x => x.GradeId).ToList()
        }, CancellationToken.None);

        // Assert
        Assert.Equal(1, Context.UserGrades.Count(ug => ug.UserId == user!.UserId));
        Assert.NotEmpty(Context.UserGrades
            .Where(ug => ug.UserId == user!.UserId &&
                         ug.Grade.GradeNumber == 8));
    }
}