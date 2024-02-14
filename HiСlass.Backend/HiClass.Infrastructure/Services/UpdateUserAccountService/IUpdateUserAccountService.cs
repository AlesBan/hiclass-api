using HiClass.Application.Dtos.UserDtos;
using HiClass.Application.Models.User;
using HiClass.Application.Models.User.Update;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace HiClass.Infrastructure.Services.UpdateUserAccountService;

public interface IUpdateUserAccountService
{
    public Task<UserProfileDto> UpdateUserPersonalInfoAsync(Guid userId, UpdatePersonalInfoRequestDto requestUserDto, IMediator mediator);
    public Task<UserProfileDto> UpdateUserImageAsync(Guid userId, UpdateImageRequestDto requestUserDto, IMediator mediator);
    public Task<UserProfileDto> UpdateUserInstitutionAsync(Guid userId, UpdateInstitutionRequestDto requestUserDto, IMediator mediator);

    public Task<UserProfileDto> UpdateUserProfessionalInfoAsync(Guid userId, UpdateProfessionalInfoRequestDto requestUserDto, IMediator mediator);

    public Task<UserProfileDto> UpdateUserEmailAsync(Guid userId, UpdateUserEmailRequestDto requestUserDto, IMediator mediator);

    public Task<UserProfileDto> UpdateUserPasswordAsync(Guid userId, UpdateUserPasswordHashRequestDto requestUserDto, IMediator mediator);
}