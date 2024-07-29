using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Helpers.UserHelper;

public interface IUserHelper
{
    public Task UpdateAsync(User user);
    public Task<User> GetBlankUserById(Guid userId, IMediator mediator);
    public Task<User> GetBlankUserWithDevicesById(Guid userId, IMediator mediator);
    public Task<User> GetFullUserById(Guid userId, IMediator mediator);
    public Task<User> GetBlankUserByEmail(string email, IMediator mediator);
    public Task<Guid> GetUserIdByClassId(Guid classId, IMediator mediator);
    public void CheckUserVerification(User user);
    public void CheckUserCreateAccountAbility(User user);
    public string GenerateVerificationCode();
    public string GeneratePasswordResetCode();
    public void CheckResetPasswordCode(User user, string code);
}