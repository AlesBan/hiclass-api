using HiClass.Application.Common.Exceptions.Database;

namespace HiClass.Persistence;

public static class DbInitializer
{
    public static void Initialize(SharedLessonDbContext context)
    {
        try
        {
            context.Database.EnsureCreated();
        }
        catch
        {
            throw new DatabaseConnectionException();
        }
    }
}