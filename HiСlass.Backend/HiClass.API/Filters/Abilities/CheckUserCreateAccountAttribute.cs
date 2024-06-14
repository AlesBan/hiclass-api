using System.Net;
using HiClass.API.Helpers.JwtHelpers;
using HiClass.Application.Common.Exceptions.User.Forbidden;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HiClass.API.Filters.Abilities;

public class CheckUserCreateAccountAttribute : TypeFilterAttribute
{
    public CheckUserCreateAccountAttribute() : base(typeof(CheckUserCreateAccountImplementation))
    {
    }

    private class CheckUserCreateAccountImplementation : IAsyncActionFilter
    {
        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var (userId, isCreateAccount) = JwtHelper.GetUserIdAndIsCreatedAccountFromClaims(context.HttpContext);

            if (isCreateAccount)
            {
                return next();
            }

            context.Result = new StatusCodeResult((int)HttpStatusCode.MethodNotAllowed);

            throw new UserHasNotAccountException(userId);
        }
    }
}