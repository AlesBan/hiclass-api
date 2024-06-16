namespace HiClass.Application.Common.Exceptions;

public class ExceptionDto
{
    public string ExceptionMessageForUi { get; set; } = string.Empty;
    public string ExceptionMessageForLogging { get; set; } = string.Empty;
}