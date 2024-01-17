using HiClass.Application.Common;
using HiClass.Application.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Wonderworld.API.Models.Authentication;

namespace HiClass.API.Filters;

public class ValidateModelStateFilterAttribute : TypeFilterAttribute
{
    public ValidateModelStateFilterAttribute(): base(typeof(ValidateModelStateFilterImplementation))
    {
        
    }
    private class ValidateModelStateFilterImplementation : Attribute, IAsyncActionFilter 
    {
        private readonly ILogger<ValidateModelStateFilterImplementation> _logger;

        public ValidateModelStateFilterImplementation(ILogger<ValidateModelStateFilterImplementation> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        AuthConstants.InvalidPayloadErrorMessage
                    }
                });
            }
            await next();
        }
    }
}
