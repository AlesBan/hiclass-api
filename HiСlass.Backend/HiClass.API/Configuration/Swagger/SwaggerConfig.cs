using HiClass.API.Filters.Swagger;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;

namespace HiClass.API.Configuration.Swagger;

public static class SwaggerConfig
{
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "HiClass API",
                Description = "ASP.NET Core Web API for HiClass",
            });
            options.EnableAnnotations();
            options.OperationFilter<SwaggerExceptionFilter>();
        }).AddSwaggerGenNewtonsoftSupport();
    }

    public static void ConfigureSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}