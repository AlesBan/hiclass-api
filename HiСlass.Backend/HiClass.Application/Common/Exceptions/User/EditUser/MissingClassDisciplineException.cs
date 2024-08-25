using HiClass.Application.Interfaces.Exceptions;
using Newtonsoft.Json;

namespace HiClass.Application.Common.Exceptions.User.EditUser;

public class MissingClassDisciplineException : Exception, IUiException
{
    public MissingClassDisciplineException(Guid classId, string disciplineTitle) : base(CreateSerializedExceptionDto(classId, disciplineTitle))
    {

    }

    private static string CreateSerializedExceptionDto(Guid classId, string disciplineId)
    {
        var exceptionDto = new ExceptionResponseDto
        {
            ExceptionMessageForUi = "Missing class discipline exception.",
            ExceptionMessageForLogging = $"Class {classId} is missing the required discipline {disciplineId}."
        };

        return JsonConvert.SerializeObject(exceptionDto);
    }
}