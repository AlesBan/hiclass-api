using System.Net;
using HiClass.API.Helpers.JwtHelpers;
using HiClass.Application.Common.Exceptions.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HiClass.API.Filters.Abilities;

public class CheckUserAbilityToCreateClassAttribute : TypeFilterAttribute
{
    public CheckUserAbilityToCreateClassAttribute() : base(typeof(CheckUserAbilityToCreateClassImplementation))
    {
    }
}

public class CheckUserAbilityToCreateClassImplementation : IAsyncActionFilter
{
    public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var positionInfo = JwtHelper.GetPositionInfoFromClaims(context.HttpContext);

        if (positionInfo.IsTeacher)
        {
            return next();
        }
        
        context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
        throw new UserUnableToCreateClassException();

    }
}