using HiClass.Application.Dtos.UserDtos;
using HiClass.Application.Dtos.UserDtos.Authentication;
using HiClass.Application.Dtos.UserDtos.Login;
using HiClass.Application.Dtos.UserDtos.ResetPassword;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.DeleteAllUsers;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.DeleteUser;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.RegisterUser;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserPasswordHash;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserResetToken;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserToken;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserVerification;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetAllUsers;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Queries.GetUserById;
using HiClass.Application.Helpers;
using HiClass.Application.Helpers.TokenHelper;
using HiClass.Application.Helpers.UserHelper;
using HiClass.Application.Models.EmailManager;
using HiClass.Application.Models.User.CreateAccount;
using HiClass.Infrastructure.Services.EmailHandlerService;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace HiClass.Infrastructure.Services.AccountServices;

public class UserAccountService : IUserAccountService
{
    private readonly ITokenHelper _tokenHelper;
    private readonly IUserHelper _userHelper;
    private readonly IEmailHandlerService _emailHandlerService;
    private IConfiguration Configuration { get; set; }

    public UserAccountService(ITokenHelper tokenHelper, IUserHelper userHelper,
        IEmailHandlerService emailHandlerService, IConfiguration configuration)
    {
        _tokenHelper = tokenHelper;
        _userHelper = userHelper;
        _emailHandlerService = emailHandlerService;
        Configuration = configuration;
    }

    public async Task<IEnumerable<UserProfileDto>> GetAllUsers(IMediator mediator)
    {
        var users = await mediator.Send(new GetAllUsersQuery());
        var userProfileDtosTasks = users.Select(async u =>
            await _userHelper.MapUserToUserProfileDto(u));

        var userProfileDtos = await Task.WhenAll(userProfileDtosTasks);
        return userProfileDtos;
    }

    public async Task<LoginResponseDto> RegisterUser(UserRegisterRequestDto requestUserDto, IMediator mediator)
    {
        var registeredUser = await mediator.Send(
            new RegisterUserCommand(
                requestUserDto.Email,
                requestUserDto.Password));

        await _emailHandlerService.SendVerificationEmail(registeredUser.Email,
            registeredUser.VerificationCode);

        var loginResponseDto = new LoginResponseDto
        {
            AccessToken = registeredUser.AccessToken,
            IsCreatedAccount = false
        };
        return loginResponseDto;
    }

    public async Task<LoginResponseDto> LoginUser(UserLoginRequestDto requestUserDto, IMediator mediator)
    {
        var user = await _userHelper.GetUserByEmail(requestUserDto.Email, mediator);

        _userHelper.CheckUserVerification(user);
        PasswordHelper.VerifyPasswordHash(user, requestUserDto.Password);

        var userProfileDto = await _userHelper.MapUserToUserProfileDto(user);

        var newToken = _tokenHelper.CreateToken(user);

        userProfileDto.AccessToken = newToken;

        await mediator.Send(new UpdateUserAccessTokenCommand(user.UserId, newToken));

        var loginResponseDto = new LoginResponseDto
        {
            AccessToken = userProfileDto.AccessToken,
            IsCreatedAccount = userProfileDto.IsCreateAccount
        };
        return loginResponseDto;
    }

    public async Task<string> ConfirmEmail(Guid userId, string code, IMediator mediator)
    {
        var user = await _userHelper.GetUserById(userId, mediator);

        var verifiedUser = await mediator.Send(
            new UpdateUserVerificationCodeCommand(user.UserId, code));

        return verifiedUser.AccessToken;
    }

    public async Task<string> ForgotPassword(string userEmail, IMediator mediator)
    {
        var user = await _userHelper.GetUserByEmail(userEmail, mediator);

        var updatedUser = await mediator.Send(new UpdateUserResetPasswordInfoCommand(user.UserId));


        await _emailHandlerService.SendResetPasswordEmail(user.Email, updatedUser.PasswordResetCode);
        return updatedUser.PasswordResetToken;
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
            new UpdateUserPasswordCommand()
            {
                UserId = user.UserId,
                Password = requestDto.Password
            });
        var newToken = _tokenHelper.CreateToken(user);

        user.AccessToken = newToken;
        await mediator.Send(new UpdateUserAccessTokenCommand(user.UserId, newToken));

        var loginResponseDtoDto = new LoginResponseDto
        {
            AccessToken = newToken,
            IsCreatedAccount = user.IsCreatedAccount
        };
        return loginResponseDtoDto;
    }

    public async Task<UserProfileDto> CreateUserAccount(Guid userId,
        CreateUserAccountRequestDto requestUserDto,
        IMediator mediator)
    {
        var user = await _userHelper.GetUserById(userId, mediator);

        _userHelper.CheckUserVerification(user);
        var userWithAccount = await _userHelper.CreateUserAccount(userId, requestUserDto, mediator);

        var userProfileDto = await _userHelper.MapUserToUserProfileDto(userWithAccount);

        return userProfileDto;
    }

    public async Task<UserProfileDto> GetUserProfile(Guid userId, IMediator mediator)
    {
        var user = await mediator.Send(new GetUserByIdQuery(userId));
        var userProfileDto = await _userHelper.MapUserToUserProfileDto(user);
        return userProfileDto;
    }

    public async Task DeleteUser(Guid userId, IMediator mediator)
    {
        await mediator.Send(new DeleteUserCommand(userId));
    }

    public async Task DeleteAllUsers(IMediator mediator)
    {
        await mediator.Send(new DeleteAllUsersCommand());
    }
}