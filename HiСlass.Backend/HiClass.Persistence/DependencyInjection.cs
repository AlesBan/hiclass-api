using HiClass.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HiClass.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISharedLessonDbContext, SharedLessonDbContext>();

        var connection = configuration["CONNECTIONSTRINGS:DB_CONNECTION"];

        services.AddDbContext<SharedLessonDbContext>(options =>
        {
            options.UseNpgsql(connection);
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            options.EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine, LogLevel.Information);
        });

        return services;
    }
}