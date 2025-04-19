using Hangfire;
using Hangfire.MemoryStorage;

namespace HiClass.API.Startup;

public static class HangfireConfiguration
{
    public static IServiceCollection AddHangfireServices(
        this IServiceCollection services)
    {
        services.AddHangfire(config => 
        {
            config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseMemoryStorage();
        });
        
        services.AddHangfireServer(options => 
        {
            options.ServerName = "HiClass.Hangfire";
            options.Queues = new[] { "default", "emails" };
        });
        
        return services;
    }
}