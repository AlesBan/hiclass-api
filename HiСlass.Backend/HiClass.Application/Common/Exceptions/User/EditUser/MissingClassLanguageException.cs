using HiClass.Application.Interfaces.Exceptions;
using Newtonsoft.Json;

namespace HiClass.Application.Common.Exceptions.User.EditUser;

public class MissingClassLanguageException : Exception, IUiException
{
    public MissingClassLanguageException(Guid classId, string languageTitle) : base(CreateSerializedExceptionDto(classId, languageTitle))
    {

    }

    private static string CreateSerializedExceptionDto(Guid classId, string languageId)
    {
        var exceptionDto = new ExceptionResponseDto
        {
            ExceptionMessageForUi = "Missing class language exception.",
            ExceptionMessageForLogging = $"Class {classId} is missing the required language {languageId}."
        };

        return JsonConvert.SerializeObject(exceptionDto);
    }
}