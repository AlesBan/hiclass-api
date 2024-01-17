using HiClass.Application.Handlers.EntityHandlers.InvitationHandlers.Commands.DeleteInvitation;
using HiClass.Application.Tests.Common;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HiClass.Application.Tests.Handlers.EntityHandlers.InvitationHandlers.Commands;

public class DeleteInvitationCommandHandlerTests : TestCommonBase
{
    [Fact]
    public async Task DeleteInvitationCommandHandler_Handle_ShouldDeleteInvitation()
    {
        // Arrange
        var invitation = await Context.Invitations
            .FirstOrDefaultAsync(i => 
                i.InvitationId == SharedLessonDbContextFactory.InvitationForDeleteId);
        var handler = new DeleteInvitationCommandHandler(Context);
        
        // Act
        await handler.Handle(new DeleteInvitationCommand
        {
            Invitation = invitation!,
        }, CancellationToken.None);
        
        // Assert
        Assert.Null(await Context.Invitations.SingleOrDefaultAsync(i =>
            i.InvitationId == SharedLessonDbContextFactory.InvitationForDeleteId));
    }
}