using HiClass.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HiClass.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<ISharedLessonDbContext, SharedLessonDbContext>();

        var connection = configuration["CONNECTIONSTRINGS:DB_CONNECTION"];
        
        services.AddEntityFrameworkNpgsql()
            .AddDbContext<SharedLessonDbContext>(options =>
                {
                    options.UseNpgsql(connection);
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                }
            );
        
        return services;
    }
}