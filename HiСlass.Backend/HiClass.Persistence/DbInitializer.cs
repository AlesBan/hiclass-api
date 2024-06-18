using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Common.Exceptions.Server;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Persistence;

public static class DbInitializer
{
    public static void Initialize(SharedLessonDbContext context)
    {
        try
        {
            context.Database.EnsureCreated();
            
            if (!context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }
        catch
        {
            throw new DatabaseConnectionException();
        }
    }
}