using AutoMapper;
using HiClass.Application.Common.Mappings;
using HiClass.Application.Interfaces;
using HiClass.Persistence;
using Xunit;

namespace HiClass.Application.Tests.Common;

public class QueryTestFixture : IDisposable
{
    public readonly SharedLessonDbContext Context;
    public readonly IMapper Mapper;

    public QueryTestFixture()
    {
        Context = SharedLessonDbContextFactory.Create();
        var configProvider = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new AssemblyMappingProfile(
                    typeof(ISharedLessonDbContext).Assembly));
        });
        
        Mapper = configProvider.CreateMapper();
    }

    public void Dispose()
    {
        SharedLessonDbContextFactory.Destroy(Context);
    }
}

[CollectionDefinition("QueryCollection")]
public class QueryCollection : ICollectionFixture<QueryTestFixture>
{
}