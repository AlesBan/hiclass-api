using Microsoft.Extensions.Logging;

namespace HiClass.Application.Common.Logging;

public static class ExceptionLogger
{
    private static ILoggerFactory _loggerFactory;

    public static void Configure(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
    }

    public static ILogger CreateLogger<T>()
    {
        return _loggerFactory.CreateLogger<T>();
    }
}