using HiClass.API.Filters;
using HiClass.API.Helpers;
using HiClass.Application.Dtos.ClassDtos;
using HiClass.Infrastructure.Services.ClassServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HiClass.API.Controllers;

[Authorize]
[CheckUserCreateAccount]
public class ClassController : BaseController
{
    private readonly IClassService _classService;
    public ClassController(IClassService classService)
    {
        _classService = classService;
    }

    [HttpPost("create-class")]
    public async Task<IActionResult> CreateClass([FromBody] CreateClassRequestDto requestClassDto)
    {
        var result = await _classService.CreateClass(UserId, requestClassDto, Mediator);
        return ResponseHelper.GetOkResult(result);
    }

    [HttpGet] 
    public async Task<IActionResult> GetClassProfile(Guid classId)
    {
       var result = await _classService.GetClassProfile(classId, Mediator);
       return ResponseHelper.GetOkResult(result);
    }

    [HttpPut("update-class/{classId:guid}")]
    public async Task<IActionResult> UpdateClass([FromBody] UpdateClassRequestDto requestClassDto, Guid classId)
    {
        var result = await _classService.UpdateClass(classId, requestClassDto, Mediator);
        return ResponseHelper.GetOkResult(result);
    }

    [HttpDelete("delete-class/{classId:guid}")]
    public async Task<IActionResult> DeleteClass(Guid classId)
    {
        await _classService.DeleteClass(classId, Mediator);
        return ResponseHelper.GetOkResult();
    }
}