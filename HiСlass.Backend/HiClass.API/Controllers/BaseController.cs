using HiClass.API.Filters.Exceptions;
using HiClass.API.Filters.Validation;
using HiClass.API.Helpers.JwtHelpers;
using HiClass.Application.Common.Exceptions.Server;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace HiClass.API.Controllers;

[Produces("application/json")]
[Route("api/[controller]")]
[ValidateModelStateFilter]
[ExceptionFilter]
[ApiController]
public class BaseController : ControllerBase
{
    protected Guid UserId => JwtHelper.GetUserIdFromClaims(HttpContext);

    protected IMediator Mediator =>
        HttpContext.RequestServices.GetService<IMediator>() ?? throw new MediatorNotFoundException();
}