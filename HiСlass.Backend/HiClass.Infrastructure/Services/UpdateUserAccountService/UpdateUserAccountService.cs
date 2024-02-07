using HiClass.Application.Common.Exceptions;
using HiClass.Application.Dtos.UserDtos;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdatePersonalInfo;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateProfessionalInfo;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserEmail;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserInstitution;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserPasswordHash;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserToken;
using HiClass.Application.Helpers.TokenHelper;
using HiClass.Application.Helpers.UserHelper;
using HiClass.Application.Interfaces.Services;
using HiClass.Application.Models.User.Update;
using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Infrastructure.Services.UpdateUserAccountService;

public class UpdateUserAccountService : IUpdateUserAccountService
{
    private readonly IUserHelper _userHelper;
    private readonly ITokenHelper _tokenHelper;

    public UpdateUserAccountService(IUserHelper userHelper, ITokenHelper tokenHelper)
    {
        _userHelper = userHelper;
        _tokenHelper = tokenHelper;
    }

    public async Task<UserProfileDto> UpdateUserPersonalInfoAsync(Guid userId,
        UpdatePersonalInfoRequestDto requestUserDto,
        IMediator mediator)
    {
        var user = await GetResultOfUpdatingUserAsync(userId, requestUserDto, mediator);

        var userProfileDto = await _userHelper.MapUserToUserProfileDto(user);

        return userProfileDto;
    }

    public async Task<UserProfileDto> UpdateUserInstitutionAsync(Guid userId, UpdateInstitutionRequestDto requestUserDto,
        IMediator mediator)
    {
        var user = await GetResultOfUpdatingUserAsync(userId, requestUserDto, mediator);

        var userProfileDto = await _userHelper.MapUserToUserProfileDto(user);

        return userProfileDto;
    }

    public async Task<UserProfileDto> UpdateUserEmailAsync(Guid userId, UpdateUserEmailRequestDto requestUserDto,
        IMediator mediator)
    {
        var user = await GetResultOfUpdatingUserAsync(userId, requestUserDto, mediator);
        var userProfileDto = await _userHelper.MapUserToUserProfileDto(user);

        var newToken = _tokenHelper.CreateToken(user);

        userProfileDto.AccessToken = newToken;

        await mediator.Send(new UpdateUserAccessTokenCommand(user.UserId, newToken));
        return userProfileDto;
    }

    public async Task<UserProfileDto> UpdateUserProfessionalInfoAsync(Guid userId,
        UpdateProfessionalInfoRequestDto requestUserDto, IMediator mediator)
    {
        var user = await GetResultOfUpdatingUserAsync(userId, requestUserDto, mediator);

        var userProfileDto = await _userHelper.MapUserToUserProfileDto(user);

        return userProfileDto;
    }

    public async Task<UserProfileDto> UpdateUserPasswordAsync(Guid userId,
        UpdateUserPasswordHashRequestDto requestUserDto,
        IMediator mediator)
    {
        var user = await GetResultOfUpdatingUserAsync(userId, requestUserDto, mediator);
        var userProfileDto = await _userHelper.MapUserToUserProfileDto(user);

        return userProfileDto;
    }

    private async Task<User> GetResultOfUpdatingUserAsync<TRequestDto>(Guid userId,
        TRequestDto requestUserDto,
        IMediator mediator)
    {
        var user = await _userHelper.GetUserById(userId, mediator);
        var updatedUser = await GetUpdatedUserAsync(user, requestUserDto, mediator);

        return updatedUser;
    }

    private static async Task<User> GetUpdatedUserAsync<TRequestDto>(User user, TRequestDto requestUserDto,
        IMediator mediator)
    {
        return requestUserDto switch
        {
            UpdatePersonalInfoRequestDto personalInfoRequestDto =>
                await mediator.Send(new UpdatePersonalInfoCommand
                {
                    UserId = user.UserId,
                    IsATeacher = personalInfoRequestDto.IsATeacher,
                    IsAnExpert = personalInfoRequestDto.IsAnExpert,
                    FirstName = personalInfoRequestDto.FirstName,
                    LastName = personalInfoRequestDto.LastName,
                    CityTitle = personalInfoRequestDto.CityTitle,
                    CountryTitle = personalInfoRequestDto.CountryTitle,
                    Description = personalInfoRequestDto.Description
                }),
            UpdateProfessionalInfoRequestDto professionalInfoRequestDto =>
                await mediator.Send(
                    new UpdateProfessionalInfoCommand
                    {
                        UserId = user.UserId,
                        LanguageTitles = professionalInfoRequestDto.Languages,
                        DisciplineTitles = professionalInfoRequestDto.Disciplines,
                        GradeNumbers = professionalInfoRequestDto.Grades
                    }),
            UpdateInstitutionRequestDto institutionRequestDto =>
                await mediator.Send(new UpdateUserInstitutionCommand
                {
                    UserId = user.UserId,
                    InstitutionTitle = institutionRequestDto.InstitutionTitle,
                    Address = institutionRequestDto.Address,
                    Types = institutionRequestDto.Types,
                }),
            UpdateUserEmailRequestDto emailRequestDto =>
                await mediator.Send(new UpdateUserEmailAndRemoveVerificationCommand
                {
                    UserId = user.UserId,
                    Email = emailRequestDto.Email
                }),
            UpdateUserPasswordHashRequestDto passwordRequestDto =>
                await mediator.Send(new UpdateUserPasswordCommand
                {
                    UserId = user.UserId,
                    Password = passwordRequestDto.Password
                }),
            _ => throw new UnknownTypeException()
        };
    }
}