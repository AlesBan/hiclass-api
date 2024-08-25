using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.Internal;

public class InvalidDisciplineIdProvidedException : Exception, IServerException
{
    public InvalidDisciplineIdProvidedException(Guid disciplineId)
        : base(
            $"Invalid discipline id provided. " +
            $"Value: {(disciplineId == Guid.Empty ? "empty" : disciplineId.ToString())}.")

    {
    }
}