using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.Invitations;

public class EqualityInvitationClassIdPropertiesException : Exception, IUiException
{
    private const string ExceptionMessageForUi = "ClassSenderId and ClassReceiverId must not be the same.";

    public EqualityInvitationClassIdPropertiesException() :
        base(CreateSerializedExceptionDto())
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