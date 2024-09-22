using HiClass.Application.Models.Images.Editing.Banner;
using HiClass.Application.Models.Images.Editing.Image;
using HiClass.Application.Models.User;
using HiClass.Application.Models.User.Editing;
using HiClass.Application.Models.User.Editing.Requests;
using MediatR;

namespace HiClass.Infrastructure.InternalServices.EditUserAccountService;

public interface IEditUserAccountService
{
    public Task<UserProfileDto> EditUserPersonalInfoAsync(Guid userId, EditPersonalInfoRequestDto requestUserDto,
        IMediator mediator);

    public Task<UserProfileDto> EditUserImageAsync(Guid userId, EditImageRequestDto requestUserDto, IMediator mediator);

    public Task<UserProfileDto> EditUserBannerImageAsync(Guid userId, EditBannerImageRequestDto requestUserDto,
        IMediator mediator);

    public Task<UserProfileDto> EditUserInstitutionAsync(Guid userId, EditInstitutionRequestDto requestUserDto,
        IMediator mediator);

    public Task<UserProfileDto> EditUserProfessionalInfoAsync(Guid userId,
        EditProfessionalInfoRequestDto requestUserDto, IMediator mediator);

    public Task<UserProfileDto> EditUserEmailAsync(Guid userId, EditUserEmailRequestDto requestUserDto,
        IMediator mediator);

    public Task<UserProfileDto> EditUserPasswordAsync(Guid userId, EditUserPasswordHashRequestDto requestUserDto,
        IMediator mediator);

    public Task<UserProfileDto> SetUserPasswordAsync(Guid userId, SetUserPasswordHashRequestDto requestUserDto,
        IMediator mediator);
}