using HiClass.Application.Dtos.UserDtos;
using HiClass.Application.Models.User.Update;
using MediatR;

namespace HiClass.Application.Interfaces.Services;

public interface IEditUserAccountService
{
    public Task<UserProfileDto> EditUserPersonalInfoAsync(Guid userId, UpdatePersonalInfoRequestDto requestUserDto, IMediator mediator);

    public Task<UserProfileDto> EditUserInstitutionAsync(Guid userId, UpdateInstitutionRequestDto requestUserDto, IMediator mediator);

    public Task<UserProfileDto> EditUserProfessionalInfoAsync(Guid userId, UpdateProfessionalInfoRequestDto requestUserDto, IMediator mediator);

    public Task<UserProfileDto> EditUserEmailAsync(Guid userId, UpdateUserEmailRequestDto requestUserDto, IMediator mediator);

    public Task<UserProfileDto> EditUserPasswordAsync(Guid userId, UpdateUserPasswordHashRequestDto requestUserDto, IMediator mediator);
}