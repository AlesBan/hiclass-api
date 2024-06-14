using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.Unknown;

public class UnknownTypeException : BaseException, IServerException
{
    private const string ExceptionMessage = "Unknown type";

    public UnknownTypeException(Guid userId) : base(ExceptionMessage, CreateMessageForLogging(userId))
    {
    }

    private static string CreateMessageForLogging(Guid userId)
    {
        return $"{userId} gets {nameof(UnknownTypeException)} exception";
    }
}
