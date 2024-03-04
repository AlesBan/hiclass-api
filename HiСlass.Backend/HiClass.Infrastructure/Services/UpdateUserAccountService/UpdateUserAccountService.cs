using AutoMapper;
using HiClass.Application.Common.Exceptions;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdatePersonalInfo;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateProfessionalInfo;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserAccessToken;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserEmail;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserImage;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserInstitution;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserPasswordHash;
using HiClass.Application.Helpers.TokenHelper;
using HiClass.Application.Helpers.UserHelper;
using HiClass.Application.Models.User;
using HiClass.Application.Models.User.Update;
using HiClass.Domain.Entities.Main;
using HiClass.Infrastructure.Services.ImageServices;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace HiClass.Infrastructure.Services.UpdateUserAccountService;

public class UpdateUserAccountService : IUpdateUserAccountService
{
    private readonly IUserHelper _userHelper;
    private readonly ITokenHelper _tokenHelper;
    private readonly IImageHandlerService _imageHandlerService;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public UpdateUserAccountService(IUserHelper userHelper, ITokenHelper tokenHelper,
        IImageHandlerService imageHandlerService, IConfiguration configuration, IMapper mapper)
    {
        _userHelper = userHelper;
        _tokenHelper = tokenHelper;
        _imageHandlerService = imageHandlerService;
        _configuration = configuration;
        _mapper = mapper;
    }

    public async Task<UserProfileDto> UpdateUserPersonalInfoAsync(Guid userId,
        UpdatePersonalInfoRequestDto requestUserDto,
        IMediator mediator)
    {
        var user = await GetResultOfUpdatingUserAsync(userId, requestUserDto, mediator);

        var userProfileDto = await _userHelper.MapUserToUserProfileDto(user);

        return userProfileDto;
    }

    public async Task<UserProfileDto> UpdateUserImageAsync(Guid userId, UpdateImageRequestDto requestUserDto,
        IMediator mediator)
    {
        var user = await GetResultOfUpdatingUserAsync(userId, requestUserDto, mediator);

        var userProfileDto = await _userHelper.MapUserToUserProfileDto(user);

        return userProfileDto;
    }

    public async Task<UserProfileDto> UpdateUserInstitutionAsync(Guid userId,
        UpdateInstitutionRequestDto requestUserDto,
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

        var createAccessTokenUserDto = _mapper.Map<CreateAccessTokenUserDto>(requestUserDto);
        var newToken = _tokenHelper.CreateToken(createAccessTokenUserDto);

        var command = new UpdateUserAccessTokenCommand()
        {
            UserId = user.UserId,
            AccessToken = newToken
        };
        var updatedUser = await mediator.Send(command);

        var userProfileDto = await _userHelper.MapUserToUserProfileDto(updatedUser);

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
        var updatedUser = await GetUpdatedUserAsync(userId, requestUserDto, mediator);

        return updatedUser;
    }

    private async Task<User> GetUpdatedUserAsync<TRequestDto>(Guid userId, TRequestDto requestUserDto,
        IMediator mediator)
    {
        switch (requestUserDto)
        {
            case UpdatePersonalInfoRequestDto personalInfoRequestDto:
                return await mediator.Send(new UpdatePersonalInfoCommand
                {
                    UserId = userId,
                    IsATeacher = personalInfoRequestDto.IsATeacher,
                    IsAnExpert = personalInfoRequestDto.IsAnExpert,
                    FirstName = personalInfoRequestDto.FirstName,
                    LastName = personalInfoRequestDto.LastName,
                    CityTitle = personalInfoRequestDto.CityTitle,
                    CountryTitle = personalInfoRequestDto.CountryTitle,
                    Description = personalInfoRequestDto.Description
                });
            case UpdateImageRequestDto imageRequestDto:
            {
                var file = imageRequestDto.ImageFormFile;
                var awsS3UpdateImageResponseDto = await _imageHandlerService.UpdateImageAsync(file,
                    _configuration["AWS_CONFIGURATION:USER_IMAGES_FOLDER"], userId.ToString());

                var command = new UpdateUserImageCommand(userId, awsS3UpdateImageResponseDto.ImageUrl);
                return await mediator.Send(command);
            }
            case UpdateProfessionalInfoRequestDto professionalInfoRequestDto:
                return await mediator.Send(new UpdateProfessionalInfoCommand
                {
                    UserId = userId,
                    LanguageTitles = professionalInfoRequestDto.Languages,
                    DisciplineTitles = professionalInfoRequestDto.Disciplines,
                    GradeNumbers = professionalInfoRequestDto.Grades
                });
            case UpdateInstitutionRequestDto institutionRequestDto:
                return await mediator.Send(new UpdateUserInstitutionCommand
                {
                    UserId = userId,
                    InstitutionTitle = institutionRequestDto.InstitutionTitle,
                    Address = institutionRequestDto.Address,
                    Types = institutionRequestDto.Types,
                });
            case UpdateUserEmailRequestDto emailRequestDto:
                return await mediator.Send(new UpdateUserEmailAndRemoveVerificationCommand
                {
                    UserId = userId, Email = emailRequestDto.Email
                });
            case UpdateUserPasswordHashRequestDto passwordRequestDto:
                return await mediator.Send(new UpdateUserPasswordCommand
                {
                    UserId = userId, Password = passwordRequestDto.Password
                });
            default:
                throw new UnknownTypeException();
        }
    }
}