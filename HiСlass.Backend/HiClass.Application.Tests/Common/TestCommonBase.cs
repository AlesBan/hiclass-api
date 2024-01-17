using HiClass.Persistence;

namespace HiClass.Application.Tests.Common;

public abstract class TestCommonBase : IDisposable
{
    protected readonly SharedLessonDbContext Context;

    public TestCommonBase()
    {
        Context = SharedLessonDbContextFactory.Create();
    }

    public void Dispose()
    {
        SharedLessonDbContextFactory.Destroy(Context);
        GC.SuppressFinalize(this);
    }
}