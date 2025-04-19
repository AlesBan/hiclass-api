using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.Extensions.DependencyInjection;

namespace HiClass.Infrastructure.Configuration;

public static class HangfireConfig
{
    public static IServiceCollection AddHangfireConfiguration(this IServiceCollection services)
    {
        services.AddHangfire(config => config
            .UseMemoryStorage()); 

        services.AddHangfireServer(options => 
        {
            options.ServerName = "HiClass.EmailServer";
            options.Queues = new[] { "emails" };
        });

        return services;
    }
}