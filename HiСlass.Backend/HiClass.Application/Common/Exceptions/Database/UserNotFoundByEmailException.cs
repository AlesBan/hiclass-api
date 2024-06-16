using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.Database;

using static Newtonsoft.Json.JsonConvert;

public class UserNotFoundByEmailException : Exception, IUiException
{
    private static string ExceptionMessageForUi(string userEmail) =>
        $"User with {userEmail} email was not found.";

    public UserNotFoundByEmailException(string userEmail) :
        base(CreateSerializedExceptionDto(userEmail))
    {
    }

    private static string CreateSerializedExceptionDto(string userEmail)
    {
        var exceptionDto = new ExceptionDto
        {
            ExceptionMessageForUi = $"ExceptionMessageForUI: " +
                                    $"{ExceptionMessageForUi(userEmail)}",
            ExceptionMessageForLogging = $"ExceptionMessageForLogging: " +
                                         $"{CreateMessageForLogging(userEmail)}"
        };

        var serializedExceptionDto = SerializeObject(exceptionDto);

        return serializedExceptionDto;
    }

    private static string CreateMessageForLogging(string userEmail)
    {
        return $"User with {userEmail} email was not found.";
    }
}