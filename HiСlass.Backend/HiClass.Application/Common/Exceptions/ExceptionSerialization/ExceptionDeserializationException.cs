namespace HiClass.Application.Common.Exceptions.ExceptionSerialization;

public class ExceptionDeserializationException : Exception
{
    public ExceptionDeserializationException(string exceptionTitle) :
        base($"Deserialization of the {exceptionTitle} exception went wrong.")
    {
    }
}