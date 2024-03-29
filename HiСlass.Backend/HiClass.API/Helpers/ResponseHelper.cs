using HiClass.API.Models;
using HiClass.API.Models.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace HiClass.API.Helpers;

public static class ResponseHelper
{
    public static IActionResult GetBadRequest(string message)
    {
        return new BadRequestObjectResult(new ResponseResult
        {
            Result = false,
            Errors = new List<string> { message },
        });
    }
    
    public static IActionResult GetExceptionObjectResult(string message)
    {
        return new ObjectResult(new ResponseResult
        {
            Result = false,
            Errors = new List<string> { message },
        });
    }

    public static OkObjectResult GetOkResult()
    {
        return new OkObjectResult(new ResponseResult
        {
            Result = true,
            Value = null
        });
    }

    public static OkObjectResult GetOkResult(object value)
    {
        return new OkObjectResult(new ResponseResult
        {
            Result = true,
            Value = value
        });
    }

    public static OkObjectResult GetAuthResultOk(string token)
    {
        return new OkObjectResult(new AuthResult
        {
            Result = true,
            Token = token
        });
    }
}