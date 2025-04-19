using Hangfire.Dashboard;

namespace HiClass.API.Filters.Hangfire;

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        // В Development разрешаем доступ без аутентификации
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
        {
            return true;
        }

        // В Production можно добавить проверку ролей/прав
        // var httpContext = context.GetHttpContext();
        // return httpContext.User.Identity.IsAuthenticated && 
        //        httpContext.User.IsInRole("Admin");
        
        return false; // По умолчанию запрещаем доступ
    }
}