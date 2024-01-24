using System.Net;
using HiClass.API.Helpers.JwtHelpers;
using HiClass.Application.Common.Exceptions.User.Forbidden;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HiClass.API.Filters.UserVerification;

public class CheckUserVerificationAttribute : TypeFilterAttribute
{
    public CheckUserVerificationAttribute() : base(typeof(CheckUserVerificationAttributeImplementation))
    {
    }

    private class CheckUserVerificationAttributeImplementation : IAsyncActionFilter
    {
        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var isVerified = JwtHelper.GetIsVerifiedFromClaims(context.HttpContext);

            if (isVerified)
            {
                return next();
            }

            context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
            
            var userId = JwtHelper.GetUserIdFromClaims(context.HttpContext);
            throw new UserNotVerifiedException(userId);
        }
    }
}