namespace HiClass.Application.Common.Exceptions;

public class ExceptionDto
{
    public string ExceptionTitle { get; set; } = string.Empty;
    public string ExceptionMessage { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}