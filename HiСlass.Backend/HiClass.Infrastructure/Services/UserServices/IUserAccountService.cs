using HiClass.Application.Models.Images.Setting;
using HiClass.Application.Models.User;
using HiClass.Application.Models.User.Authentication;
using HiClass.Application.Models.User.CreateAccount;
using HiClass.Application.Models.User.EmailVerification;
using HiClass.Application.Models.User.EmailVerification.ReVerification;
using HiClass.Application.Models.User.Login;
using HiClass.Application.Models.User.PasswordHandling;
using MediatR;

namespace HiClass.Infrastructure.Services.UserServices;

public interface IUserAccountService
{
    public Task<UserProfileDto> GetUserProfile(Guid userId, IMediator mediator);
    public Task<IEnumerable<FullUserProfileDto>> GetAllUsers(IMediator mediator);
    public Task<LoginResponseDto> RegisterUser(UserRegisterRequestDto requestUserDto, IMediator mediator);
    public Task<LoginResponseDto> LoginUser(UserLoginRequestDto requestUserDto, IMediator mediator);
    public Task<EmailVerificationResponseDto> VerifyEmailUsingId(Guid userId, string code, IMediator mediator);
    public Task<EmailVerificationResponseDto> VerifyEmailUsingEmail(string email, string code, IMediator mediator);
    public Task CreateAndReSendVerificationCode(EmailReVerificationRequestDto requestDto, IMediator mediator);
    public Task<ForgotPasswordResponseDto> ForgotPassword(string userEmail, IMediator mediator);
    public Task CheckResetPasswordCode(Guid userId, string code, IMediator mediator);
    public Task<LoginResponseDto> ResetPassword(Guid userId, ResetPasswordRequestDto requestDto, IMediator mediator);
    public Task<CreateAccountUserProfileDto> CreateUserAccount(Guid userId, CreateUserAccountRequestDto requestUserDto, IMediator mediator);
    public Task<SetImageResponseDto> SetUserImage(Guid userId, SetImageRequestDto requestDto, IMediator mediator);
    public Task<SetImageResponseDto> SetUserBannerImage(Guid userId, SetImageRequestDto requestDto, IMediator mediator);
    public Task DeleteUser(Guid userId, IMediator mediator);
    public Task DeleteAllUsers(IMediator mediator);
    
    
}