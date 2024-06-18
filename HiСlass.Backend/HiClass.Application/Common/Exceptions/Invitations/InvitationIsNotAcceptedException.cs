using HiClass.Application.Interfaces.Exceptions;
using HiClass.Domain.Enums;
using static Newtonsoft.Json.JsonConvert;

namespace HiClass.Application.Common.Exceptions.Invitations;

public class InvitationIsNotAcceptedException : Exception, IUiException
{
    public InvitationIsNotAcceptedException(Guid userId, InvitationStatus status) :
        base(CreateSerializedExceptionDto(userId, status))
    {
    }

    private static string CreateSerializedExceptionDto(Guid userId, InvitationStatus status)
    {
        var exceptionDto = new ExceptionResponseDto
        {
            ExceptionMessageForUi = "Invitation is not accepted, status: " + status,
            ExceptionMessageForLogging = CreateMessageForLogging(userId, status)
        };

        var serializedExceptionDto = SerializeObject(exceptionDto);

        return serializedExceptionDto;
    }

    private static string CreateMessageForLogging(Guid userId, InvitationStatus status)
    {
        return $"{userId} gets {nameof(InvitationIsNotAcceptedException)} exception: " +
               "Invitation is not accepted, status: " + status;
    }
}