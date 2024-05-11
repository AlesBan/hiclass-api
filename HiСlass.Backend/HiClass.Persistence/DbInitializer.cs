using HiClass.Application.Common.Exceptions.Database;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Persistence;

public static class DbInitializer
{
    public static void Initialize(SharedLessonDbContext context)
    {
        try
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.Database.Migrate();
        }
        catch
        {
            throw new DatabaseConnectionException();
        }
    }
}