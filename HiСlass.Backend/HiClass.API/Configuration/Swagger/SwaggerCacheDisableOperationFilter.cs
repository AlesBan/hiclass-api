using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HiClass.API.Swagger;

public class SwaggerCacheDisableOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Responses.Add("Cache-Control", new OpenApiResponse { Description = "no-cache" });
        operation.Responses.Add("Pragma", new OpenApiResponse { Description = "no-cache" });
        operation.Responses.Add("Expires", new OpenApiResponse { Description = "-1" });
    }
}