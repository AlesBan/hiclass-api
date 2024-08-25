using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.Internal;

public class InvalidUserIdProvidedException : Exception, IServerException
{
    public InvalidUserIdProvidedException(Guid userId)
        : base($"Invalid user id: {(userId == Guid.Empty ? "empty" : userId.ToString())}.")
    {
    }
}