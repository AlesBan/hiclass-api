using HiClass.API.Helpers;
using HiClass.Application.Models.Images.Editing.Banner;
using HiClass.Application.Models.Images.Setting;
using HiClass.Infrastructure.Services.AccountServices;
using HiClass.Infrastructure.Services.ClassServices;
using HiClass.Infrastructure.Services.EditUserAccountService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EditImageRequestDto = HiClass.Application.Models.Images.Editing.Image.EditImageRequestDto;

namespace HiClass.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : BaseController
    {
        private readonly IUserAccountService _userAccountService;
        private readonly IClassService _classService;
        private readonly IEditUserAccountService _editUserAccountService;

        public ImageController(IUserAccountService userAccountService, IClassService classService,
            IEditUserAccountService editUserAccountService)
        {
            _userAccountService = userAccountService;
            _classService = classService;
            _editUserAccountService = editUserAccountService;
        }

        [Authorize]
        [HttpPut("set-user-image")]
        public async Task<IActionResult> SetUserImage([FromForm] SetImageRequestDto requestUserDto)
        {
            var result = await _userAccountService.SetUserImage(UserId, requestUserDto, Mediator);
            return ResponseHelper.GetOkResult(result);
        }

        [Authorize]
        [HttpPut("edit-user-image")]
        public async Task<IActionResult> EditUserImage(
            [FromForm] EditImageRequestDto requestDto)
        {
            var result = await _editUserAccountService.EditUserImageAsync(UserId, requestDto, Mediator);
            return ResponseHelper.GetOkResult(result);
        }

        [Authorize]
        [HttpPut("set-user-banner-image")]
        public async Task<IActionResult> SetUserBannerImage([FromForm] SetImageRequestDto requestUserDto)
        {
            var result = await _userAccountService.SetUserBannerImage(UserId, requestUserDto, Mediator);
            return ResponseHelper.GetOkResult(result);
        }

        [Authorize]
        [HttpPut("edit-user-banner-image")]
        public async Task<IActionResult> EditUserBannerImage([FromForm] EditBannerImageRequestDto requestUserDto)
        {
            var result = await _editUserAccountService.EditUserBannerImageAsync(UserId, requestUserDto, Mediator);
            return ResponseHelper.GetOkResult(result);
        }

        [Authorize]
        [HttpPut("set-class-image/{classId:guid}")]
        public async Task<IActionResult> UploadClassImage([FromRoute] Guid classId,
            [FromForm] SetImageRequestDto requestUserDto)
        {
            var result = await _classService.SetClassImage(classId, requestUserDto, Mediator);
            return ResponseHelper.GetOkResult(result);
        }


        [Authorize]
        [HttpPut("edit-class-image/{classId:guid}")]
        public async Task<IActionResult> SetClassImage([FromRoute] Guid classId,
            [FromForm] EditImageRequestDto requestClassDto)
        {
            var result = await _classService.UpdateClassImage(classId, requestClassDto, Mediator);
            return ResponseHelper.GetOkResult(result);
        }
    }
}