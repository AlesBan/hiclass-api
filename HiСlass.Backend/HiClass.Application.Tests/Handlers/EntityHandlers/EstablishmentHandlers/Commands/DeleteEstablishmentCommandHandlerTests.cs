using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Handlers.EntityHandlers.InstitutionHandlers.Commands.DeleteEstablishment;
using HiClass.Application.Tests.Common;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HiClass.Application.Tests.Handlers.EntityHandlers.EstablishmentHandlers.Commands;

public class DeleteInstitutionCommandHandlerTests: TestCommonBase
{
    [Fact]
    public async Task DeleteEstablishmentCommandHandler_Handle_ShouldDeleteEstablishment()
    {
        // Arrange
        var establishmentId = SharedLessonDbContextFactory.EstablishmentForDeleteId;
        var handler = new DeleteInstitutionCommandHandler(Context);
        
        // Act
        await handler.Handle(new DeleteInstitutionCommand()
            {
                EstablishmentId = establishmentId
            }, 
            CancellationToken.None);
        
        // Assert
        Assert.Null(await Context.Institutions
            .SingleOrDefaultAsync(e => 
            e.InstitutionId == establishmentId));
    }
    
    [Fact]
    public async Task DeleteEstablishmentCommandHandler_Handle_FailOnWrongId()
    {
        // Arrange
        var handler = new DeleteInstitutionCommandHandler(Context);

        // Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await handler.Handle(new DeleteInstitutionCommand { EstablishmentId = Guid.NewGuid() },
                CancellationToken.None));
    }
}