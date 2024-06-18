using System.Net;
using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Common.Exceptions.Server;
using HiClass.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HiClass.API.Middleware;


public class DatabaseConnectionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            using var scope = context.RequestServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SharedLessonDbContext>();
            if (await dbContext.Database.CanConnectAsync())
            {
                var dbConnection = dbContext.Database.GetDbConnection();
                var connectionString = dbConnection.ConnectionString;
                await next(context);
            }
            else
            {
                throw new DatabaseConnectionException();
            }
        }
        catch (DatabaseConnectionException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
            await context.Response.WriteAsync(ex.Message);
        }
    }
}

public static class DatabaseConnectionMiddlewareExtensions
{
    public static IApplicationBuilder UseDatabaseConnection(this IApplicationBuilder app)
    {
        return app.UseMiddleware<DatabaseConnectionMiddleware>();
    }
}