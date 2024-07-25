using HiClass.Application.Models.Images.Setting;
using HiClass.Application.Models.User;
using HiClass.Application.Models.User.Authentication;
using HiClass.Application.Models.User.CreateAccount;
using HiClass.Application.Models.User.EmailVerification;
using HiClass.Application.Models.User.EmailVerification.ReVerification;
using HiClass.Application.Models.User.PasswordHandling;
using HiClass.Application.Models.User.RevokeToken;
using MediatR;

namespace HiClass.Infrastructure.InternalServices.UserServices;

public interface IUserAccountService
{
    public Task<UserProfileDto> GetUserProfile(Guid userId, IMediator mediator);
    public Task<IEnumerable<FullUserProfileDto>> GetAllUsers(IMediator mediator);
    public Task<TokenModelResponseDto> Register(RegisterRequestDto requestDto, IMediator mediator);
    public Task<TokenModelResponseDto> Login(LoginRequestDto requestDto, IMediator mediator);
    public Task<TokenModelResponseDto> RefreshToken(Guid userId, RefreshTokenRequestDto requestDto, IMediator mediator);
    public Task LogOut(Guid userId, LogOutRequestDto requestDto, IMediator mediator);
    public Task RevokeRefreshTokenAsync(Guid userId, RevokeTokenRequestDto requestDto, IMediator mediator);

    public Task<TokenModelResponseDto>
        VerifyEmailUsingEmail(EmailVerificationRequestDto requestDto, IMediator mediator);
    public Task CreateAndReSendVerificationCode(EmailReVerificationRequestDto requestDto, IMediator mediator);
    public Task GenerateAndSetResetPasswordCodeAsync(string userEmail, IMediator mediator);
    public Task<AccessTokenDto> CheckResetPasswordCodeAsync(string userEmail, string code, IMediator mediator);
    public Task<TokenModelResponseDto> ResetPasswordAsync(Guid userId, ResetPasswordRequestDto requestDto, IMediator mediator);
    public Task<TokenModelResponseDto> CreateUserAccount(Guid userId,
        CreateUserAccountRequestDto requestDto, IMediator mediator);
    public Task<SetImageResponseDto> SetUserImage(Guid userId, SetImageRequestDto requestDto, IMediator mediator);
    public Task<SetImageResponseDto> SetUserBannerImage(Guid userId, SetImageRequestDto requestDto, IMediator mediator);
    public Task DeleteUser(Guid userId, IMediator mediator);
    public Task DeleteAllUsers(IMediator mediator);
}