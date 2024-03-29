using HiClass.API.Filters.Abilities;
using HiClass.API.Helpers;
using HiClass.Application.Models.Class;
using HiClass.Application.Models.Class.EditClassDtos;
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
    public async Task<IActionResult> CreateClass([FromForm] CreateClassRequestDto requestClassDto)
    {
        var result = await _classService.CreateClass(UserId, requestClassDto, Mediator);
        return ResponseHelper.GetOkResult(result);
    }

    [HttpGet("class-profile/{classId:guid}")]
    public async Task<IActionResult> GetClassProfile([FromRoute] Guid classId)
    {
        var result = await _classService.GetClassProfile(classId, Mediator);
        return ResponseHelper.GetOkResult(result);
    }

    [HttpPut("edit-class/{classId:guid}")]
    public async Task<IActionResult> UpdateClass([FromRoute] Guid classId,
        [FromForm] EditClassRequestDto requestClassDto)
    {
        var result = await _classService.UpdateClass(classId, requestClassDto, Mediator);
        return ResponseHelper.GetOkResult(result);
    }

    [HttpDelete("delete-class/{classId:guid}")]
    public async Task<IActionResult> DeleteClass([FromRoute] Guid classId)
    {
        await _classService.DeleteClass(classId, Mediator);
        return ResponseHelper.GetOkResult();
    }
}