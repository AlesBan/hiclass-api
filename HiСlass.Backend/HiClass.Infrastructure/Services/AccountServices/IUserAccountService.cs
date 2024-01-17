using HiClass.Application.Dtos.UserDtos;
using HiClass.Application.Dtos.UserDtos.Authentication;
using HiClass.Application.Dtos.UserDtos.CreateAccount;
using HiClass.Application.Dtos.UserDtos.Login;
using HiClass.Application.Dtos.UserDtos.ResetPassword;
using MediatR;

namespace HiClass.Infrastructure.Services.AccountServices;

public interface IUserAccountService
{
    public Task<UserProfileDto> GetUserProfile(Guid userId, IMediator mediator);
    public Task<IEnumerable<UserProfileDto>> GetAllUsers(IMediator mediator);
    public Task<LoginResponseDto> RegisterUser(UserRegisterRequestDto requestUserDto, IMediator mediator);
    public Task<LoginResponseDto> LoginUser(UserLoginRequestDto requestUserDto, IMediator mediator);
    public Task<string> ConfirmEmail(Guid userId, string token, IMediator mediator);
    public Task<string> ForgotPassword(string userEmail, IMediator mediator);
    public Task CheckResetPasswordCode(Guid userId, string code, IMediator mediator);
    public Task<LoginResponseDto> ResetPassword(Guid userId, ResetPasswordRequestDto requestDto, IMediator mediator);
    public Task<UserProfileDto> CreateUserAccount(Guid userId, CreateUserAccountRequestDto requestUserDto, IMediator mediator);
    public Task DeleteUser(Guid userId, IMediator mediator);
    public Task DeleteAllUsers(IMediator mediator);
    
    
}