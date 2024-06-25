using AutoMapper;
using HiClass.Application.Common.Exceptions.Unknown;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.EditPersonalInfo;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.EditProfessionalInfo;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.EditUserBannerImage;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.EditUserEmail;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.EditUserImage;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.EditUserInstitution;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.EditUserPasswordHash;
using HiClass.Application.Helpers.TokenHelper;
using HiClass.Application.Models.Images.Editing.Banner;
using HiClass.Application.Models.Images.Editing.Image;
using HiClass.Application.Models.User;
using HiClass.Application.Models.User.Editing;
using HiClass.Application.Models.User.Editing.Requests;
using HiClass.Domain.Entities.Main;
using HiClass.Infrastructure.InternalServices.ImageServices;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace HiClass.Infrastructure.InternalServices.EditUserAccountService;

public class EditUserAccountService : IEditUserAccountService
{
    private readonly ITokenHelper _tokenHelper;
    private readonly IImageHandlerService _imageHandlerService;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public EditUserAccountService(ITokenHelper tokenHelper,
        IImageHandlerService imageHandlerService, IConfiguration configuration, IMapper mapper)
    {
        _tokenHelper = tokenHelper;
        _imageHandlerService = imageHandlerService;
        _configuration = configuration;
        _mapper = mapper;
    }

    public async Task<UserProfileDto> EditUserPersonalInfoAsync(Guid userId,
        EditPersonalInfoRequestDto requestUserDto,
        IMediator mediator)
    {
        var user = await GetResultOfUpdatingUserAsync(userId, requestUserDto, mediator);

        var userProfileDto = _mapper.Map<UserProfileDto>(user);

        return userProfileDto;
    }

    public async Task<UserProfileDto> EditUserImageAsync(Guid userId, EditImageRequestDto requestUserDto,
        IMediator mediator)
    {
        var user = await GetResultOfUpdatingUserAsync(userId, requestUserDto, mediator);

        var userProfileDto = _mapper.Map<UserProfileDto>(user);

        return userProfileDto;
    }

    public async Task<UserProfileDto> EditUserBannerImageAsync(Guid userId, EditBannerImageRequestDto requestUserDto,
        IMediator mediator)
    {
        var user = await GetResultOfUpdatingUserAsync(userId, requestUserDto, mediator);

        var userProfileDto = _mapper.Map<UserProfileDto>(user);

        return userProfileDto;
    }

    public async Task<UserProfileDto> EditUserInstitutionAsync(Guid userId,
        EditInstitutionRequestDto requestUserDto,
        IMediator mediator)
    {
        var user = await GetResultOfUpdatingUserAsync(userId, requestUserDto, mediator);

        var userProfileDto = _mapper.Map<UserProfileDto>(user);

        return userProfileDto;
    }

    public async Task<UserProfileDto> EditUserEmailAsync(Guid userId, EditUserEmailRequestDto requestUserDto,
        IMediator mediator)
    {
        var user = await GetResultOfUpdatingUserAsync(userId, requestUserDto, mediator);

        var userProfileDto = _mapper.Map<UserProfileDto>(user);

        return userProfileDto;
    }

    public async Task<UserProfileDto> EditUserProfessionalInfoAsync(Guid userId,
        EditProfessionalInfoRequestDto requestUserDto, IMediator mediator)
    {
        var user = await GetResultOfUpdatingUserAsync(userId, requestUserDto, mediator);

        var userProfileDto = _mapper.Map<UserProfileDto>(user);

        return userProfileDto;
    }

    public async Task<UserProfileDto> EditUserPasswordAsync(Guid userId,
        EditUserPasswordHashRequestDto requestUserDto,
        IMediator mediator)
    {
        var user = await GetResultOfUpdatingUserAsync(userId, requestUserDto, mediator);
        var userProfileDto = _mapper.Map<UserProfileDto>(user);

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
            case EditPersonalInfoRequestDto personalInfoRequestDto:
                return await mediator.Send(new EditPersonalInfoCommand
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
            case EditImageRequestDto imageRequestDto:
            {
                var file = imageRequestDto.ImageFormFile;
                var awsS3UpdateImageResponseDto = await _imageHandlerService.UpdateImageAsync(file,
                    _configuration["AWS_CONFIGURATION:USER_IMAGES_FOLDER"], userId.ToString());

                var command = new EditUserImageCommand(userId, awsS3UpdateImageResponseDto.ImageUrl);
                return await mediator.Send(command);
            }
            case EditBannerImageRequestDto imageRequestDto:
            {
                var file = imageRequestDto.ImageFormFile;
                var awsS3UpdateImageResponseDto = await _imageHandlerService.UpdateImageAsync(file,
                    _configuration["AWS_CONFIGURATION:USER_BANNER_IMAGES_FOLDER"], userId.ToString());

                var command = new EditUserBannerImageCommand(userId, awsS3UpdateImageResponseDto.ImageUrl);
                return await mediator.Send(command);
            }
            case EditProfessionalInfoRequestDto professionalInfoRequestDto:
                return await mediator.Send(new EditProfessionalInfoCommand
                {
                    UserId = userId,
                    LanguageTitles = professionalInfoRequestDto.Languages,
                    DisciplineTitles = professionalInfoRequestDto.Disciplines,
                    GradeNumbers = professionalInfoRequestDto.Grades
                });
            case EditInstitutionRequestDto institutionRequestDto:
                return await mediator.Send(new EditUserInstitutionCommand
                {
                    UserId = userId,
                    InstitutionTitle = institutionRequestDto.InstitutionTitle,
                    Address = institutionRequestDto.Address,
                    Types = institutionRequestDto.Types,
                });
            case EditUserEmailRequestDto emailRequestDto:
                return await mediator.Send(new EditUserEmailAndRemoveVerificationCommand
                {
                    UserId = userId, Email = emailRequestDto.Email
                });
            case EditUserPasswordHashRequestDto passwordRequestDto:
                return await mediator.Send(new EditUserPasswordCommand
                {
                    UserId = userId, 
                    OldPassword = passwordRequestDto.OldPassword,
                    NewPassword = passwordRequestDto.Password
                });
            default:
                throw new UnknownTypeException(userId);
        }
    }
}