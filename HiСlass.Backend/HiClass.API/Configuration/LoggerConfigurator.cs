using HiClass.Application.Common.Logging;

namespace HiClass.API.Configuration;

public static class LoggerConfigurator
{
    public static void ConfigureLogger(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

        ExceptionLogger.Configure(loggerFactory);
    }
}
