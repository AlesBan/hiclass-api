using System.ComponentModel.DataAnnotations;
using HiClass.Application.Models.User.Authentication;
using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.RegisterUser;

public class RegisterUserCommand : IRequest<User>
{
    public RegisterUserCommand(UserRegisterRequestDto userRegisterRequestDto)
    {
        UserRegisterRequestDto = userRegisterRequestDto;
    }

    public UserRegisterRequestDto UserRegisterRequestDto  { get; set; }
}