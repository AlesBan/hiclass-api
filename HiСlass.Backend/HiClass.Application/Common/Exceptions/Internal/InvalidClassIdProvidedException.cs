using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.Internal;

public class InvalidClassIdProvidedException : Exception, IServerException
{
    public InvalidClassIdProvidedException(Guid classId)
        : base($"Invalid class id: {(classId == Guid.Empty ? "empty" : classId.ToString())}.")
    {
    }
}