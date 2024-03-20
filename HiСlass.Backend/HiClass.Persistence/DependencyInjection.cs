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

        
        var connection =
             "Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=postgres;Integrated Security=True;Pooling=True;";
        
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