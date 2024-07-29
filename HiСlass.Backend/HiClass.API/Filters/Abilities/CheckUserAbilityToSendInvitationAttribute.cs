using System.Net;
using HiClass.API.Helpers.JwtHelpers;
using HiClass.Application.Common.Exceptions.User.Forbidden;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HiClass.API.Filters.Abilities;

public class CheckUserAbilityToSendInvitationAttribute : TypeFilterAttribute
{
    public CheckUserAbilityToSendInvitationAttribute() : base(
        typeof(CheckUserAbilityToSendInvitationAttributeImplementation))
    {
    }
}

public class CheckUserAbilityToSendInvitationAttributeImplementation : IAsyncActionFilter
{
    public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var positionInfo = JwtHelper.GetPositionInfoFromClaims(context.HttpContext);

        if (positionInfo.IsTeacher)
        {
            return next();
        }

        context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
        throw new InvalidPositionForSendingInvitationException();
    }
}