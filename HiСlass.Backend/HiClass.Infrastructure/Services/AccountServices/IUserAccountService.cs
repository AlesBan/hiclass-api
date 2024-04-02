using HiClass.Application.Dtos.UserDtos;
using HiClass.Application.Dtos.UserDtos.Authentication;
using HiClass.Application.Models.Images;
using HiClass.Application.Models.Images.Editing;
using HiClass.Application.Models.Images.Setting;
using HiClass.Application.Models.User;
using HiClass.Application.Models.User.Authentication;
using HiClass.Application.Models.User.CreateAccount;
using HiClass.Application.Models.User.EmailVerification;
using HiClass.Application.Models.User.Login;
using HiClass.Application.Models.User.PasswordHandling;
using MediatR;

namespace HiClass.Infrastructure.Services.AccountServices;

public interface IUserAccountService
{
    public Task<UserProfileDto> GetUserProfile(Guid userId, IMediator mediator);
    public Task<IEnumerable<FullUserProfileDto>> GetAllUsers(IMediator mediator);
    public Task<LoginResponseDto> RegisterUser(UserRegisterRequestDto requestUserDto, IMediator mediator);
    public Task<LoginResponseDto> LoginUser(UserLoginRequestDto requestUserDto, IMediator mediator);
    public Task<EmailVerificationResponseDto> VerifyEmail(Guid userId, string token, IMediator mediator);
    public Task CreateAndReSendVerificationCode(Guid userId, IMediator mediator);
    public Task<ForgotPasswordResponseDto> ForgotPassword(string userEmail, IMediator mediator);
    public Task CheckResetPasswordCode(Guid userId, string code, IMediator mediator);
    public Task<LoginResponseDto> ResetPassword(Guid userId, ResetPasswordRequestDto requestDto, IMediator mediator);
    public Task<CreateAccountUserProfileDto> CreateUserAccount(Guid userId, CreateUserAccountRequestDto requestUserDto, IMediator mediator);
    public Task<SetImageResponseDto> SetUserImage(Guid userId, SetImageRequestDto requestDto, IMediator mediator);
    public Task<SetImageResponseDto> SetUserBannerImage(Guid userId, SetImageRequestDto requestDto, IMediator mediator);
    public Task DeleteUser(Guid userId, IMediator mediator);
    public Task DeleteAllUsers(IMediator mediator);
    
    
}