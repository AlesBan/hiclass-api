using System.ComponentModel.DataAnnotations;
using HiClass.Application.Models.User.Authentication;
using HiClass.Application.Models.User.Registration;
using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.RegisterUser;

public class RegisterUserCommand : IRequest<RegisterUserCommandResponse>
{
    public RegisterUserCommand(RegisterRequestDto userRegisterRequestDto)
    {
        UserRegisterRequestDto = userRegisterRequestDto;
    }

    public RegisterRequestDto UserRegisterRequestDto  { get; set; }
}