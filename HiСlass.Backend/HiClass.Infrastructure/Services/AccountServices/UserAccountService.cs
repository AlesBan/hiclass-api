using AutoMapper;
using HiClass.Application.Dtos.UserDtos.Authentication;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.CreateUserAccount;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.DeleteAllUsers;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.DeleteUser;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.EditUserPasswordHash;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.LoginUser;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.RegisterUser;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.SetUserImage;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserVerification;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserVerificationCode;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetAllUsers;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetUserById;
using HiClass.Application.Helpers.DataHelper;
using HiClass.Application.Helpers.TokenHelper;
using HiClass.Application.Helpers.UserHelper;
using HiClass.Application.Interfaces;
using HiClass.Application.Interfaces.Services;
using HiClass.Application.Models.Images;
using HiClass.Application.Models.Images.Editing;
using HiClass.Application.Models.Images.Editing.Image;
using HiClass.Application.Models.Images.Setting;
using HiClass.Application.Models.User;
using HiClass.Application.Models.User.Authentication;
using HiClass.Application.Models.User.CreateAccount;
using HiClass.Application.Models.User.EmailVerification;
using HiClass.Application.Models.User.Login;
using HiClass.Application.Models.User.PasswordHandling;
using HiClass.Domain.Entities.Main;
using HiClass.Infrastructure.Services.ImageServices;
using HiClass.Infrastructure.Services.NotificationHandlerService;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HiClass.Infrastructure.Services.AccountServices;

public class UserAccountService : IUserAccountService
{
    private readonly ITokenHelper _tokenHelper;
    private readonly IUserHelper _userHelper;
    private readonly IUserDataHelper _dataUserHelper;
    private readonly IEmailHandlerService _emailHandlerService;
    private readonly IConfiguration _configuration;
    private readonly IImageHandlerService _imageHandlerService;
    private readonly IMapper _mapper;
    private readonly ISharedLessonDbContext _context;
    private readonly INotificationHandlerService _notificationHandlerService;

    public UserAccountService(ITokenHelper tokenHelper, IUserHelper userHelper,
        IEmailHandlerService emailHandlerService, IConfiguration configuration,
        IUserDataHelper dataUserHelper, IImageHandlerService imageHandlerService,
        IMapper mapper, ISharedLessonDbContext context, INotificationHandlerService notificationHandlerService)
    {
        _tokenHelper = tokenHelper;
        _userHelper = userHelper;
        _emailHandlerService = emailHandlerService;
        _configuration = configuration;
        _dataUserHelper = dataUserHelper;
        _imageHandlerService = imageHandlerService;
        _mapper = mapper;
        _context = context;
        _notificationHandlerService = notificationHandlerService;
    }

    public async Task<UserProfileDto> GetUserProfile(Guid userId, IMediator mediator)
    {
        var user = await mediator.Send(new GetUserByIdQuery(userId));

        var userProfileDto = await _userHelper.MapUserToUserProfileDto(user);
        return userProfileDto;
    }

    public async Task<IEnumerable<FullUserProfileDto>> GetAllUsers(IMediator mediator)
    {
        var users = await mediator.Send(new GetAllUsersQuery());
        var userProfileDtosTasks = users.Select(async u =>
            await _userHelper.MapUserToFullUserProfileDto(u));

        var userProfileDtos = await Task.WhenAll(userProfileDtosTasks);
        return userProfileDtos;
    }

    public async Task<LoginResponseDto> RegisterUser(UserRegisterRequestDto requestUserDto, IMediator mediator)
    {
        var registeredUser = await mediator.Send(
            new RegisterUserCommand(requestUserDto));

        await _emailHandlerService.SendVerificationEmail(registeredUser.Email,
            registeredUser.VerificationCode);

        var loginResponseDto = new LoginResponseDto
        {
            AccessToken = registeredUser.AccessToken,
            IsCreatedAccount = false
        };
        
        // try
        // {
        //     _notificationHandlerService.SendMessage("test");
        //
        //     _notificationHandlerService.ScheduleMessage(registeredUser.Email, DateTime.Now.AddSeconds(20));
        // }
        // finally
        // {
        // }

        return loginResponseDto;
    }

    public async Task<LoginResponseDto> LoginUser(UserLoginRequestDto requestUserDto, IMediator mediator)
    {
        var command = new LoginUserCommand(requestUserDto);

        var loginResponseDto = await mediator.Send(command);

        return loginResponseDto;
    }

    public async Task<EmailVerificationResponseDto> VerifyEmail(Guid userId, string code, IMediator mediator)
    {
        var command = new UpdateUserVerificationCommand()
        {
            UserId = userId,
            VerificationCode = code
        };

        var newAccessToken = await mediator.Send(command);

        var emailVerificationResponseDto = new EmailVerificationResponseDto()
        {
            AccessToken = newAccessToken
        };

        return emailVerificationResponseDto;
    }

    public async Task CreateAndReSendVerificationCode(Guid userId, IMediator mediator)
    {
        var user = await _userHelper.GetUserById(userId, mediator);
        var newVerificationCode = _userHelper.GenerateVerificationCode();

        var command = new UpdateUserVerificationCodeCommand()
        {
            UserId = user.UserId,
            VerificationCode = newVerificationCode
        };

        await mediator.Send(command);

        await _emailHandlerService.SendVerificationEmail(user.Email,
            newVerificationCode);
    }

    public async Task<ForgotPasswordResponseDto> ForgotPassword(string userEmail, IMediator mediator)
    {
        var user = await _userHelper.GetUserByEmail(userEmail, mediator);

        var accessTokenUserDto = _mapper.Map<CreateAccessTokenUserDto>(user);
        var newAccessToken = _tokenHelper.CreateToken(accessTokenUserDto);

        user.PasswordResetToken = newAccessToken;
        user.ResetTokenExpires = DateTime.UtcNow.AddHours(4);

        user.PasswordResetCode = _userHelper.GeneratePasswordResetCode();

        _context.Users.Attach(user).State = EntityState.Modified;
        await _context.SaveChangesAsync(CancellationToken.None);

        await _emailHandlerService.SendResetPasswordEmail(user.Email, user.PasswordResetCode);

        return new ForgotPasswordResponseDto()
        {
            PasswordResetCode = user.PasswordResetCode,
            PasswordResetToken = newAccessToken
        };
    }

    public async Task CheckResetPasswordCode(Guid userId, string code, IMediator mediator)
    {
        var user = await _userHelper.GetUserById(userId, mediator);
        _userHelper.CheckResetTokenExpiration(user);
        _userHelper.CheckResetPasswordCode(user, code);
    }

    public async Task<LoginResponseDto> ResetPassword(Guid userId, ResetPasswordRequestDto requestDto,
        IMediator mediator)
    {
        var user = await _userHelper.GetUserById(userId, mediator);
        _userHelper.CheckResetTokenExpiration(user);

        await mediator.Send(
            new EditUserPasswordCommand()
            {
                UserId = user.UserId,
                Password = requestDto.Password
            });

        var tokenUserDto = _mapper.Map<CreateAccessTokenUserDto>(user);
        var newToken = _tokenHelper.CreateToken(tokenUserDto);


        user.AccessToken = newToken;
        await _context.SaveChangesAsync(CancellationToken.None);


        var loginResponseDtoDto = new LoginResponseDto
        {
            AccessToken = newToken,
            IsCreatedAccount = user.IsCreatedAccount
        };
        return loginResponseDtoDto;
    }

    public async Task<CreateAccountUserProfileDto> CreateUserAccount(Guid userId,
        CreateUserAccountRequestDto requestDto,
        IMediator mediator)
    {
        var user = await _userHelper.GetUserById(userId, mediator);

        _userHelper.CheckUserVerification(user);

        _userHelper.CheckUserCreateAccountAbility(user);

        var userWithAccount = await GetCreatedUserAccount(userId, user.Email, requestDto, mediator);

        var userProfileDto = await _userHelper.MapUserToCreateAccountUserProfileDto(userWithAccount);

        return userProfileDto;
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

    public async Task DeleteUser(Guid userId, IMediator mediator)
    {
        await mediator.Send(new DeleteUserCommand(userId));
    }

    public async Task DeleteAllUsers(IMediator mediator)
    {
        await mediator.Send(new DeleteAllUsersCommand());
    }

    private async Task<User> GetCreatedUserAccount(Guid userId, string userEmail,
        CreateUserAccountRequestDto requestUserDto, IMediator mediator)
    {
        var command = await GetCreateUserAccountCommand(userId, userEmail, requestUserDto, mediator);
        return await mediator.Send(command);
    }

    private async Task<CreateUserAccountCommand> GetCreateUserAccountCommand(Guid userId,
        string userEmail, CreateUserAccountRequestDto requestUserDto, IMediator mediator)
    {
        var country = await _dataUserHelper.GetCountryByTitle(requestUserDto.CountryLocation, mediator);
        var city = await _dataUserHelper.GetCityByCountryId(country.CountryId, requestUserDto.CityLocation, mediator);
        var institution = await _dataUserHelper.GetInstitution(requestUserDto, mediator);
        var disciplines = await _dataUserHelper.GetDisciplinesByTitles(requestUserDto.DisciplineTitles, mediator);
        var languages = await _dataUserHelper.GetLanguagesByTitles(requestUserDto.LanguageTitles, mediator);
        var grades = await _dataUserHelper.GetGradesByNumbers(requestUserDto.GradesEnumerable, mediator);

        CreateAccessTokenUserDto user = new()
        {
            UserId = userId,
            Email = userEmail,
            IsVerified = true,
            IsCreatedAccount = true,
            IsATeacher = requestUserDto.IsATeacher,
            IsAnExpert = requestUserDto.IsAnExpert,
        };

        var newToken = _tokenHelper.CreateToken(user);

        var query = new CreateUserAccountCommand
        {
            UserId = userId,
            AccessToken = newToken,
            FirstName = requestUserDto.FirstName,
            LastName = requestUserDto.LastName,
            IsATeacher = requestUserDto.IsATeacher,
            IsAnExpert = requestUserDto.IsAnExpert,
            CountryId = country.CountryId,
            CityId = city.CityId,
            InstitutionId = institution.InstitutionId,
            DisciplineIds = disciplines.Select(d => d.DisciplineId).ToList(),
            LanguageIds = languages.Select(l => l.LanguageId).ToList(),
            GradeIds = grades.Select(g => g.GradeId).ToList()
        };

        return query;
    }
}