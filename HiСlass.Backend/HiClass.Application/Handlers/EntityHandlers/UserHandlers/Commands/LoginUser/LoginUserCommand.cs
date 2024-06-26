using HiClass.Application.Models.User.Authentication;
using HiClass.Application.Models.User.Login;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.LoginUser;

public class LoginUserCommand : IRequest<LoginUserCommandResponse>
{
    public UserLoginRequestDto UserLoginRequestDto { get; set; }

    public LoginUserCommand(UserLoginRequestDto userLoginRequestDto)
    {
        UserLoginRequestDto = userLoginRequestDto;
    }
}