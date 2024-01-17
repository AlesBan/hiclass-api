using HiClass.Application.Handlers.EntityHandlers.InstitutionHandlers.Commands.CreateInstitution;
using HiClass.Application.Tests.Common;
using HiClass.Domain.Entities.Job;
using HiClass.Domain.Enums.EntityTypes;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HiClass.Application.Tests.Handlers.EntityHandlers.EstablishmentHandlers.Commands;

public class CreateInstitutionCommandHandlerTests : TestCommonBase
{
    [Fact]
    public async Task CreateEstablishmentCommandHandler_Handle_ShouldCreateEstablishment()
    {
        // Arrange
        const string address = "address";
        const string title = "Establishment1";
        var typeTitle = InstitutionTypes.School.ToString();

        var command = new CreateInstitutionCommand
        {
            InstitutionTitle = title,
            Address = address,
            Types = new List<string> { typeTitle }
        
        };

        var handler = new CreateInstitutionCommandHandler(Context);

        // Act
        var establishment = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(await Context.Institutions
            .SingleOrDefaultAsync(e =>
                e.InstitutionId == establishment.InstitutionId));
    }
}