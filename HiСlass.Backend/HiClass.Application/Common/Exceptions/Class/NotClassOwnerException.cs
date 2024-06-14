using HiClass.Application.Common.Logging;
using HiClass.Application.Interfaces.Exceptions;
using Microsoft.Extensions.Logging;

namespace HiClass.Application.Common.Exceptions.Class;

public class NotClassOwnerException : BaseException, IUiException
{
    private const string ExceptionMessage = $"User is not the owner of class.";

    public NotClassOwnerException(Guid userId, Guid classId) : base(ExceptionMessage, CreateMessageForLogging(userId, classId))
    {
    }

    private static string CreateMessageForLogging(Guid userId, Guid classId)
    {
        return $"{userId} gets {nameof(NotClassOwnerException)} exception: " +
               $"User {userId} attempted to delete class {classId} but is not the owner.";
    }
}