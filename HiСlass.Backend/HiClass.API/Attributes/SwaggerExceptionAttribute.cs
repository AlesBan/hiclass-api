namespace HiClass.API.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class SwaggerExceptionAttribute : Attribute
{
    public Type ExceptionType { get; }
    public int StatusCode { get; }
    public string Description { get; }

    public SwaggerExceptionAttribute(Type exceptionType, int statusCode, string description)
    {
        ExceptionType = exceptionType;
        StatusCode = statusCode;
        Description = description;
    }
}