using AutoMapper;
using HiClass.Application.Common.Mappings;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetUserProfileListByDefaultSearchRequest;
using HiClass.Application.Models.Search;
using HiClass.Application.Tests.Common;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HiClass.Application.Tests.Handlers.EntityHandlers.UserHandlers.Queries;

public class GetUserProfileListByDefaultSearchQueryCommandHandlerTests : TestCommonBase
{
    [Fact]
    public async Task Handle_GetUserProfileList_ReturnsUserProfileDtoList()
    {
        // Arrange
        var discipline1 = Context.Disciplines.SingleAsync(d => d
            .Title == "Mathematics").Result;
        var discipline2 = Context.Disciplines.SingleAsync(d => d
            .Title == "Chemistry").Result;

        var command = new GetUserListByDefaultSearchRequestCommand
        {
            DisciplineIds = new List<Guid> { discipline1.DisciplineId, discipline2.DisciplineId },
            CountryId = Context.Countries.SingleAsync(c => c
                .Title == "CountryA").Result.CountryId
        };

        var cancellationToken = new CancellationToken();

        var mapperConfiguration = new MapperConfiguration(cfg => { cfg.AddProfile<AssemblyMappingProfile>(); });

        var mapper = mapperConfiguration.CreateMapper();

        var handler = new GetUserListByDefaultSearchRequestCommandHandler(Context);

        // Act
        var result = await handler.Handle(command, cancellationToken);

        // Assert
        Assert.NotNull(result);
    }
}