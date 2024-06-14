using HiClass.Application.Common.Logging;
using Microsoft.Extensions.Logging;

namespace HiClass.Application.Common.Exceptions;

public abstract class BaseException : Exception
{
    private static readonly Lazy<ILogger> _logger = new Lazy<ILogger>(ExceptionLogger.CreateLogger<BaseException>);

    protected BaseException(string exceptionMessage, string exceptionMessageForLogging) : base(exceptionMessage)
    {
        _logger.Value.LogError(exceptionMessageForLogging);
    }
}