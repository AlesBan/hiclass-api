using HiClass.Application.Dtos.UserDtos;
using HiClass.Application.Models.User;
using HiClass.Application.Models.User.CreateAccount;
using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Helpers.UserHelper;

public interface IUserHelper
{
    public Task<User> GetUserById(Guid userId, IMediator mediator);
    public Task<User> GetUserByEmail(string email, IMediator mediator);
    public Task<Guid> GetUserIdByClassId(Guid classId, IMediator mediator);
    public void CheckUserVerification(User user);
    public void CheckUserCreateAccountAbility(User user);
    public Task<FullUserProfileDto> MapUserToFullUserProfileDto(User user);
    public Task<UserProfileDto> MapUserToUserProfileDto(User user);
    public Task<CreateAccountUserProfileDto> MapUserToCreateAccountUserProfileDto(User user);
    public string GenerateVerificationCode();
    public void CheckResetTokenValidation(User user);
    public string GeneratePasswordResetCode();
    public void CheckResetPasswordCode(User user, string code);
}