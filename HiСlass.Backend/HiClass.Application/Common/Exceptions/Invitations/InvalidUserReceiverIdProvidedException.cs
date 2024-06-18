using HiClass.Application.Interfaces.Exceptions;
using static Newtonsoft.Json.JsonConvert;

namespace HiClass.Application.Common.Exceptions.Invitations;

public class InvalidUserReceiverIdProvidedException : Exception, IUiException
{
    private const string ExceptionMessageForUi = "Invalid user receiver id provided";

    public InvalidUserReceiverIdProvidedException(Guid invitationId, Guid userId, Guid userReceiverId) :
        base(CreateSerializedExceptionDto(invitationId, userId, userReceiverId))
    {
    }

    private static string CreateSerializedExceptionDto(Guid invitationId, Guid userId, Guid userReceiverId)
    {
        var exceptionDto = new ExceptionResponseDto
        {
            ExceptionMessageForUi = ExceptionMessageForUi,
            ExceptionMessageForLogging = CreateMessageForLogging(invitationId, userId, userReceiverId)
        };

        var serializedExceptionDto = SerializeObject(exceptionDto);

        return serializedExceptionDto;
    }

    private static string CreateMessageForLogging(Guid invitationId, Guid userId, Guid userReceiverId)
    {
        return $"For invitation with {invitationId} id, userId {userId} is not consistent with " +
               $"invitation userReceiverId {userReceiverId}";
    }
}