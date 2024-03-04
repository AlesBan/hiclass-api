using HiClass.API.Helpers;
using HiClass.Application.Models.Class.SetImageDtos;
using HiClass.Application.Models.Class.UpdateClassDtos.UpdateImageDtos;
using HiClass.Application.Models.User.CreateAccount;
using HiClass.Application.Models.User.Update;
using HiClass.Infrastructure.Services.AccountServices;
using HiClass.Infrastructure.Services.ClassServices;
using HiClass.Infrastructure.Services.UpdateUserAccountService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HiClass.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : BaseController
    {
        private readonly IUserAccountService _userAccountService;
        private readonly IClassService _classService;
        private readonly IUpdateUserAccountService _editUserAccountService;

        public ImageController(IUserAccountService userAccountService, IClassService classService,
            IUpdateUserAccountService editUserAccountService)
        {
            _userAccountService = userAccountService;
            _classService = classService;
            _editUserAccountService = editUserAccountService;
        }

        [Authorize]
        [HttpPut("set-user-image")]
        public async Task<IActionResult> SetUserImage([FromForm] SetUserImageRequestDto requestUserDto)
        {
            var result = await _userAccountService.SetUserImage(UserId, requestUserDto, Mediator);
            return ResponseHelper.GetOkResult(result);
        }

        [Authorize]
        [HttpPut("set-class-image/{classId:guid}")]
        public async Task<IActionResult> UploadClassImage([FromForm] SetClassImageRequestDto requestUserDto,
            Guid classId)
        {
            var result = await _classService.SetClassImage(classId, requestUserDto, Mediator);
            return ResponseHelper.GetOkResult(result);
        }

        [HttpPut("update-user-image")]
        public async Task<IActionResult> EditUserImage([FromForm] UpdateImageRequestDto requestDto)
        {
            var result = await _editUserAccountService.UpdateUserImageAsync(UserId, requestDto, Mediator);
            return ResponseHelper.GetOkResult(result);
        }

        [HttpPut("update-class-image/{classId:guid}")]
        public async Task<IActionResult> SetClassImage([FromForm] UpdateClassImageRequestDto requestClassDto, Guid classId)
        {
            var result = await _classService.UpdateClassImage(classId, requestClassDto, Mediator);
            return ResponseHelper.GetOkResult(result);
        }
    }
}