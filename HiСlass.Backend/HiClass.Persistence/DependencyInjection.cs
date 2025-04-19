using HiClass.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HiClass.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddScoped<ISharedLessonDbContext, SharedLessonDbContext>();

        var connection = configuration["CONNECTIONSTRINGS:DB_CONNECTION"];

        services.AddDbContext<SharedLessonDbContext>(options =>
        {
            options.UseNpgsql(connection);
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            
            // Включение чувствительного логирования только для ошибок
            if (configuration.GetValue<bool>("EnableDetailedDbLogging", false))
            {
                options.EnableSensitiveDataLogging();
            }

            // Логирование только ошибок, критических ошибок и предупреждений
            options.LogTo(
                message => Console.WriteLine($"DB: {message}"),
                LogLevel.Warning, // Минимальный уровень логирования
                DbContextLoggerOptions.None); // Дополнительные опции
        });

        return services;
    }
}