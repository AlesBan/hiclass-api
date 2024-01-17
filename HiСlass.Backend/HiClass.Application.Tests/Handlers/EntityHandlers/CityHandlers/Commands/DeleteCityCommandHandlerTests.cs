using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Handlers.EntityHandlers.CityHandlers.Commands.DeleteCity;
using HiClass.Application.Tests.Common;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HiClass.Application.Tests.Handlers.EntityHandlers.CityHandlers.Commands;

public class DeleteCityCommandHandlerTests : TestCommonBase
{
    [Fact]
    public async Task DeleteCityCommandHandler_Handle_ShouldDeleteCity()
    {
        // Arrange
        var cityId = SharedLessonDbContextFactory.CityForDeleteId;
        var handler = new DeleteCityCommandHandler(Context);
        
        // Act
        await handler.Handle(new DeleteCityCommand { CityId = cityId }, 
            CancellationToken.None);
        
        // Assert
        Assert.Null(await Context.Cities.SingleOrDefaultAsync(c => c.CityId == cityId));
    }
    
    [Fact]
    public async Task DeleteCityCommandHandler_Handle_FailOnWrongId()
    {
        // Arrange
        var handler = new DeleteCityCommandHandler(Context);

        // Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await handler.Handle(new DeleteCityCommand { CityId = Guid.NewGuid() },
                CancellationToken.None));
    }
}