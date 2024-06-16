using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.Unknown;

public class UnknownTypeException : Exception, IServerException
{
    private const string ExceptionMessage = "Unknown type";

    public UnknownTypeException(Guid userId) : base()
    {
    }

    private static string CreateMessageForLogging(Guid userId)
    {
        return $"{userId} gets {nameof(UnknownTypeException)} exception";
    }
}
