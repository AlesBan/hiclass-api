using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.Invitations;

public class EqualityInvitationUserIdPropertiesException : Exception, IUiException
{
    private const string ExceptionMessageForUi = "UserSenderId and UserReceiverId must not be the same.";

    public EqualityInvitationUserIdPropertiesException() : base(CreateSerializedExceptionDto())
    {
    }

    private static string CreateSerializedExceptionDto()
    {
        var exceptionDto = new ExceptionResponseDto
        {
            ExceptionMessageForUi = ExceptionMessageForUi,
            ExceptionMessageForLogging = ExceptionMessageForUi
        };

        var serializedExceptionDto = Newtonsoft.Json.JsonConvert.SerializeObject(exceptionDto);

        return serializedExceptionDto;
    }
}