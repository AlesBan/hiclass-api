using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.Properties;

public class NullPropertyException : Exception, IUiException
{
    public NullPropertyException(string propertyTitle) : base(CreateSerializedExceptionDto(propertyTitle))
    {
    }

    private static string CreateSerializedExceptionDto(string propertyTitle)
    {
        var exceptionDto = new ExceptionResponseDto
        {
            ExceptionMessageForUi = $"Property {propertyTitle} cannot be null!",
            ExceptionMessageForLogging = $"Property {propertyTitle} cannot be null!",
        };

        var serializedExceptionDto = Newtonsoft.Json.JsonConvert.SerializeObject(exceptionDto);

        return serializedExceptionDto;
    }
}