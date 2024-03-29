using HiClass.Application.Handlers.EntityHandlers.FeedbackHandlers.Commands.CreateFeedback;
using HiClass.Application.Tests.Common;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HiClass.Application.Tests.Handlers.EntityHandlers.FeedbackHandlers.Commands;

public class CreateFeedbackCommandHandlerTests : TestCommonBase
{
    [Fact]
    public async Task CreateFeedbackCommandHandler_Handle_ShouldCreateFeedback()
    {
        // Arrange
        var invitation = await Context.Invitations
            .FirstOrDefaultAsync(i =>
                i.InvitationId == SharedLessonDbContextFactory.InvitationAId);
        const bool wasTheJointLesson = true;
        string? reasonForNotConducting = null;
        const string feedbackText = "Feedback text";
        const int rating = 5;

        var handler = new CreateFeedbackCommandHandler(Context);

        // Act
        await handler.Handle(new CreateFeedbackCommand
        {
            InvitationId = invitation.InvitationId,
            WasTheJointLesson = wasTheJointLesson,
            ReasonForNotConducting = reasonForNotConducting,
            FeedbackText = feedbackText,
            Rating = rating,
        }, CancellationToken.None);

        // Assert
        Assert.NotNull(await Context.Feedbacks.FirstOrDefaultAsync(f =>
            f.InvitationId == invitation.InvitationId));
    }
}