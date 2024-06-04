using HiClass.API.Attributes;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HiClass.API.Filters.Swagger;

public class SwaggerExceptionFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var controllerActionDescriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;
        var swaggerExceptionAttributes = controllerActionDescriptor
            .MethodInfo
            .GetCustomAttributes(typeof(SwaggerExceptionAttribute), inherit: true)
            .Cast<SwaggerExceptionAttribute>();

        foreach (var swaggerExceptionAttribute in swaggerExceptionAttributes)
        {
            var statusCode = swaggerExceptionAttribute.StatusCode;
            var exceptionType = swaggerExceptionAttribute.ExceptionType;
            var description = swaggerExceptionAttribute.Description;

            operation.Responses.Add(statusCode.ToString(), new OpenApiResponse
            {
                Description = description,
                Content = new Dictionary<string, OpenApiMediaType>()
            });

            if (operation.Responses.TryGetValue(statusCode.ToString(), out var response))
            {
                response.Content.Add("application/json", new OpenApiMediaType());
                if (exceptionType != null)
                {
                    response.Content["application/json"].Schema = context.SchemaGenerator.GenerateSchema(exceptionType, context.SchemaRepository);
                }
            }
        }
    }
}