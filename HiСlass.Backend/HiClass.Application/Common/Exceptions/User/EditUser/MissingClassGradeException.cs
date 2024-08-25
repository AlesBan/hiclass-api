using HiClass.Application.Interfaces.Exceptions;
using Newtonsoft.Json;
using static Newtonsoft.Json.JsonConvert;

namespace HiClass.Application.Common.Exceptions.User.EditUser;

public class MissingClassGradeException : Exception, IUiException
{
    public MissingClassGradeException(Guid classId, int gradeNumber) : base(CreateSerializedExceptionDto(classId, gradeNumber))
    {
    }

    private static string CreateSerializedExceptionDto(Guid classId, int gradeId)
    {
        var exceptionDto = new ExceptionResponseDto
        {
            ExceptionMessageForUi = "Missing class grade exception.",
            ExceptionMessageForLogging = $"Class {classId} is missing the required grade {gradeId}."
        };

        return SerializeObject(exceptionDto);
    }
}
