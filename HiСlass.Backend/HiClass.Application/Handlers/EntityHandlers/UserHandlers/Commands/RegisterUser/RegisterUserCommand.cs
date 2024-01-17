using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.RegisterUser;

public class RegisterUserCommand : IRequest<User>
{
    public string Email { get; set; } 
    public string Password { get; set; }

    public RegisterUserCommand(string email, string password)
    {
        Email = email;
        Password = password;
    }
}