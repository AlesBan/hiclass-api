using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.Internal;

public class InvalidLanguageIdListProvidedException : Exception, IServerException
{
    public InvalidLanguageIdListProvidedException(string message) : base(message)
    {
    }
}