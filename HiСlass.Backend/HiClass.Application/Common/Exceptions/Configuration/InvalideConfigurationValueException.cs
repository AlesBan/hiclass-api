using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.Configuration;

public class InvalidConfigurationValueException : Exception, IServerException
{
    public InvalidConfigurationValueException(string configElementName) : base(
        CreateMessageForLogging(configElementName))
    {
    }

    private static string CreateMessageForLogging(string configElementName)
    {
        return
            $"Invalid configuration value for '{configElementName}' detected. Please check the configuration settings.";
    }
}