using HiClass.Application.Handlers.EntityHandlers.FeedbackHandlers.Commands.DeleteFeedback;
using HiClass.Application.Tests.Common;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HiClass.Application.Tests.Handlers.EntityHandlers.FeedbackHandlers.Commands;

public class DeleteFeedbackCommandHandlerTests : TestCommonBase
{
    [Fact]
    public async Task DeleteFeedbackCommandHandler_Handle_ShouldDeleteFeedback()
    {
        // Arrange
        var feedback = await Context.Reviews.FirstOrDefaultAsync(f=>
            f.ReviewId == SharedLessonDbContextFactory.FeedbackForDeleteId);
        
        var handler = new DeleteFeedbackCommandHandler(Context);
        
        // Act
        await handler.Handle(new DeleteFeedbackCommand
        {
            Feedback = feedback!,
        }, CancellationToken.None);
        
        // Assert
        Assert.Null(await Context.Reviews.SingleOrDefaultAsync(f =>
            f.ReviewId == SharedLessonDbContextFactory.FeedbackForDeleteId));
    }
}