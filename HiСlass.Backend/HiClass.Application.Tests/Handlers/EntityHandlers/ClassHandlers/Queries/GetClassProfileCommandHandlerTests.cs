using AutoMapper;
using HiClass.Application.Dtos.ClassDtos;
using HiClass.Application.Handlers.EntityHandlers.ClassHandlers.Queries.GetClassProfile;
using HiClass.Application.Models.Class;
using HiClass.Application.Tests.Common;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace HiClass.Application.Tests.Handlers.EntityHandlers.ClassHandlers.Queries;
[Collection("QueryCollection")]
public class GetClassProfileCommandHandlerTests : TestCommonBase
{
    private readonly IMapper _mapper;

    public GetClassProfileCommandHandlerTests(QueryTestFixture fixture)
    {
        _mapper = fixture.Mapper;
    }

    [Fact]
    public async Task Handle_ShouldReturnClassProfile()
    {
        // Arrange
        var @class = await Context.Classes
            .SingleOrDefaultAsync(c => 
            c.ClassId == SharedLessonDbContextFactory.ClassAId);
        
        var handler = new GetClassProfileCommandHandler(_mapper);
        
        //Act
        var result = await handler.Handle(new GetClassProfileCommand
        {
            Class = @class!
        }, CancellationToken.None);
        
        // Assert
        Assert.NotNull(result);
        result.ShouldBeOfType<ClassProfileDto>();
        Assert.NotNull(result.UserFullName);
        result.UserFullName.ShouldBe(@class.User.FullName);
        Assert.NotNull(result.PhotoUrl);
        result.PhotoUrl.ShouldBe(@class.PhotoUrl);
    }
}