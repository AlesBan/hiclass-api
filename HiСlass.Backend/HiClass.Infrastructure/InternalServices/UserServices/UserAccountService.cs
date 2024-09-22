using AutoMapper;
using HiClass.Application.Handlers.Authentication.RefreshTokenHandlers.Commands;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.CreateUserAccount;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.DeleteAllUsers;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.DeleteUser;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.GenerateAndSetResetPasswordCode;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.LoginOrRegisterByEmailAndRefreshToken;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.LoginUser;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.LogOutUser;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.RegisterUser;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.RevokeRefreshToken;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.SetUserBannerImage;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.SetUserImage;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserPassword;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserVerification;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserVerificationCode;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetAllUsers;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetFullUserById;
using HiClass.Application.Helpers.DataHelper;
using HiClass.Application.Helpers.TokenHelper;
using HiClass.Application.Helpers.UserHelper;
using HiClass.Application.Models.Images.Setting;
using HiClass.Application.Models.User;
using HiClass.Application.Models.User.Authentication;
using HiClass.Application.Models.User.CreateAccount;
using HiClass.Application.Models.User.EmailVerification;
using HiClass.Application.Models.User.EmailVerification.ReVerification;
using HiClass.Application.Models.User.PasswordHandling;
using HiClass.Application.Models.User.RevokeToken;
using HiClass.Infrastructure.IntegrationServices.EmailHandlerService;
using HiClass.Infrastructure.InternalServices.ImageServices;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace HiClass.Infrastructure.InternalServices.UserServices;

public class UserAccountService : IUserAccountService
{
    private readonly ITokenHelper _tokenHelper;
    private readonly IUserHelper _userHelper;
    private readonly IDataForUserHelper _dataUserHelper;
    private readonly IEmailHandlerService _emailHandlerService;
    private readonly IConfiguration _configuration;
    private readonly IImageHandlerService _imageHandlerService;
    private readonly IMapper _mapper;

    public UserAccountService(ITokenHelper tokenHelper, IUserHelper userHelper,
        IEmailHandlerService emailHandlerService, IConfiguration configuration,
        IDataForUserHelper dataUserHelper, IImageHandlerService imageHandlerService,
        IMapper mapper)
    {
        _tokenHelper = tokenHelper;
        _userHelper = userHelper;
        _emailHandlerService = emailHandlerService;
        _configuration = configuration;
        _dataUserHelper = dataUserHelper;
        _imageHandlerService = imageHandlerService;
        _mapper = mapper;
    }

    public async Task<UserProfileDto> GetUserProfile(Guid userId, IMediator mediator)
    {
        var user = await mediator.Send(new GetFullUserByIdQuery(userId));

        var userProfileDto = _mapper.Map<UserProfileDto>(user);
        return userProfileDto;
    }

    public async Task<IEnumerable<FullUserProfileDto>> GetAllUsers(IMediator mediator)
    {
        var users = await mediator.Send(new GetAllUsersQuery());
        var userProfileDtos = users.Select(user => _mapper.Map<FullUserProfileDto>(user));

        return userProfileDtos;
    }

    public async Task<TokenModelResponseDto> Register(RegisterRequestDto requestUserDto, IMediator mediator)
    {
        var registeredUserCommandResponse = await mediator.Send(
            new RegisterUserCommand(requestUserDto));

        await _emailHandlerService.SendVerificationEmail(requestUserDto.Email,
            registeredUserCommandResponse.VerificationCode);

        var tokenModelResponseDto = new TokenModelResponseDto
        {
            AccessToken = registeredUserCommandResponse.AccessToken,
            RefreshToken = registeredUserCommandResponse.RefreshToken
        };

        return tokenModelResponseDto;
    }

    public async Task<TokenModelResponseDto> Login(LoginRequestDto requestUserDto, IMediator mediator)
    {
        var command = new LoginAndRefreshTokenCommand
        {
            Email = requestUserDto.Email,
            Password = requestUserDto.Password,
            DeviceToken = requestUserDto.DeviceToken
        };

        var tokenModelResponseDto = await mediator.Send(command);

        return tokenModelResponseDto;
    }

    public async Task<TokenModelResponseDto> LoginOrRegister(string email, string deviceToken, IMediator mediator)
    {
        var command = new GoogleLoginOrRegisterByEmailAndRefreshTokenCommand
        {
            Email = email,
            DeviceToken = deviceToken
        };
        
        var tokenModelResponseDto = await mediator.Send(command);
        return tokenModelResponseDto;
    }

    public async Task<TokenModelResponseDto> RefreshToken(Guid userId, RefreshTokenRequestDto requestDto,
        IMediator mediator)
    {
        var command = new RefreshTokenCommand
        {
            UserId = userId,
            RefreshToken = requestDto.RefreshToken,
            DeviceToken = requestDto.DeviceToken
        };

        var tokenModelResponseDto = await mediator.Send(command);
        return tokenModelResponseDto;
    }

    public async Task LogOut(Guid userId, LogOutRequestDto requestDto, IMediator mediator)
    {
        var command = new LogOutUserCommand
        {
            UserId = userId,
            DeviceToken = requestDto.DeviceToken
        };

        await mediator.Send(command);
    }

    public async Task RevokeRefreshTokenAsync(Guid userId, RevokeTokenRequestDto requestDto, IMediator mediator)
    {
        var command = new RevokeRefreshTokenCommand()
        {
            UserId = userId,
            DeviceToken = requestDto.DeviceToken
        };

        await mediator.Send(command);
    }

    public async Task<TokenModelResponseDto> VerifyEmailUsingEmail(EmailVerificationRequestDto requestDto,
        IMediator mediator)
    {
        var command = new UpdateUserVerificationAndRefreshTokenCommand()
        {
            DeviceToken = requestDto.DeviceToken,
            Email = requestDto.Email,
            VerificationCode = requestDto.VerificationCode
        };

        var tokenModelResponseDto = await mediator.Send(command);

        return tokenModelResponseDto;
    }

    public async Task CreateAndReSendVerificationCode(EmailReVerificationRequestDto requestDto, IMediator mediator)
    {
        var command = new UpdateUserVerificationCodeCommand()
        {
            Email = requestDto.Email
        };

        var verificationCode = await mediator.Send(command);

        await _emailHandlerService.SendVerificationEmail(requestDto.Email, verificationCode);
    }

    public async Task GenerateAndSetResetPasswordCodeAsync(string userEmail, IMediator mediator)
    {
        var command = new UpdateUserResetPasswordCodeCommand
        {
            Email = userEmail
        };
        var resetPasswordCode = await mediator.Send(command);

        await _emailHandlerService.SendResetPasswordEmail(userEmail, resetPasswordCode);
    }

    public async Task<AccessTokenDto> CheckResetPasswordCodeAsync(string userEmail, string code, IMediator mediator)
    {
        var user = await _userHelper.GetBlankUserByEmail(userEmail, mediator);
        _userHelper.CheckResetPasswordCode(user, code);

        var tokenUserDto = _mapper.Map<CreateTokenDto>(user);
        var newAccessToken = _tokenHelper.CreateAccessToken(tokenUserDto);

        return new AccessTokenDto(newAccessToken);
    }

    public async Task<TokenModelResponseDto> ResetPasswordAsync(Guid userId, ResetPasswordRequestDto requestDto,
        IMediator mediator)
    {
        var user = await _userHelper.GetBlankUserById(userId, mediator);

        var tokenModelResponseDto = await mediator.Send(
            new UpdateUserPasswordCommand()
            {
                UserId = user.UserId,
                NewPassword = requestDto.NewPassword,
                DeviceToken = requestDto.DeviceToken
            });

        return tokenModelResponseDto;
    }

    public async Task<TokenModelResponseDto> CreateUserAccount(Guid userId,
        CreateUserAccountRequestDto requestDto,
        IMediator mediator)
    {
        var user = await _userHelper.GetBlankUserById(userId, mediator);

        _userHelper.CheckUserVerification(user);

        _userHelper.CheckUserCreateAccountAbility(user);

        var command = await GetCreateUserAccountCommand(userId, requestDto, mediator);
        var tokenModelResponseDto = await mediator.Send(command);

        return tokenModelResponseDto;
    }

    public async Task<SetImageResponseDto> SetUserImage(Guid userId,
        SetImageRequestDto requestDto, IMediator mediator)
    {
        var awsS3UploadImageResponseDto = await _imageHandlerService.UploadImageAsync(requestDto.ImageFormFile,
            _configuration["AWS_CONFIGURATION:USER_IMAGES_FOLDER"], userId.ToString());
        var imageUrl = awsS3UploadImageResponseDto.ImageUrl;

        var command = new SetUserImageCommand()
        {
            UserId = userId,
            ImageUrl = imageUrl
        };

        var result = await mediator.Send(command);

        return new SetImageResponseDto()
        {
            ImageUrl = result
        };
    }

    public async Task<SetImageResponseDto> SetUserBannerImage(Guid userId, SetImageRequestDto requestDto,
        IMediator mediator)
    {
        var awsS3UploadImageResponseDto = await _imageHandlerService.UploadImageAsync(requestDto.ImageFormFile,
            _configuration["AWS_CONFIGURATION:USER_BANNER_IMAGES_FOLDER"], userId.ToString());
        var imageUrl = awsS3UploadImageResponseDto.ImageUrl;

        var command = new SetUserBannerImageCommand()
        {
            UserId = userId,
            ImageUrl = imageUrl
        };

        var result = await mediator.Send(command);

        return new SetImageResponseDto()
        {
            ImageUrl = result
        };
    }

    public async Task DeleteUser(Guid userId, IMediator mediator)
    {
        await mediator.Send(new DeleteUserCommand(userId));
    }

    public async Task DeleteAllUsers(IMediator mediator)
    {
        await mediator.Send(new DeleteAllUsersCommand());
    }

    private async Task<CreateUserAccountCommand> GetCreateUserAccountCommand(Guid userId,
        CreateUserAccountRequestDto requestUserDto, IMediator mediator)
    {
        var country = await _dataUserHelper.GetCountryByTitle(requestUserDto.CountryLocation, mediator);
        var city = await _dataUserHelper.GetCityByCountryId(country.CountryId, requestUserDto.CityLocation, mediator);
        var institution = await _dataUserHelper.GetInstitution(requestUserDto, mediator);
        var disciplines = await _dataUserHelper.GetDisciplinesByTitles(requestUserDto.DisciplineTitles, mediator);
        var languages = await _dataUserHelper.GetLanguagesByTitles(requestUserDto.LanguageTitles, mediator);
        var grades = await _dataUserHelper.GetGradesByNumbers(requestUserDto.GradesEnumerable, mediator);

        var query = new CreateUserAccountCommand
        {
            UserId = userId,
            DeviceToken = requestUserDto.DeviceToken,
            FirstName = requestUserDto.FirstName,
            LastName = requestUserDto.LastName,
            IsATeacher = requestUserDto.IsATeacher,
            IsAnExpert = requestUserDto.IsAnExpert,
            CountryId = country.CountryId,
            CityId = city.CityId,
            InstitutionId = institution.InstitutionId,
            DisciplineIds = disciplines.Select(d => d.DisciplineId).ToList(),
            LanguageIds = languages.Select(l => l.LanguageId).ToList(),
            GradeIds = grades.Select(g => g.GradeId).ToList(),
        };

        return query;
    }
}