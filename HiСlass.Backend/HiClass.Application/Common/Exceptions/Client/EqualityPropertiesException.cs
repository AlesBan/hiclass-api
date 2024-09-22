using HiClass.Application.Interfaces.Exceptions;

namespace HiClass.Application.Common.Exceptions.Client
{
    public class EqualityPropertiesException : Exception, IUiException
    {
        public EqualityPropertiesException(string message) : base(CreateSerializedExceptionDto(message))
        {
        }

        private static string CreateSerializedExceptionDto(string message)
        {
            var exceptionDto = new ExceptionResponseDto
            {
                ExceptionMessageForUi = message,
                ExceptionMessageForLogging = message
            };

            var serializedExceptionDto = Newtonsoft.Json.JsonConvert.SerializeObject(exceptionDto);

            return serializedExceptionDto;
        }
    }
}
