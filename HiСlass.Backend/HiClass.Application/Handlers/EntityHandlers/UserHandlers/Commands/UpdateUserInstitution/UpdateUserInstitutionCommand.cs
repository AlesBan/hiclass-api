using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserInstitution;

public class UpdateUserInstitutionCommand : IRequest<User>
{
    public Guid UserId { get; set; }
    public string InstitutionTitle { get; set; }
    public string Address { get; set; }
    public IEnumerable<string> Types { get; set; }

    public UpdateUserInstitutionCommand(Guid userId, string institutionTitle, string address, IEnumerable<string> types)
    {
        UserId = userId;
        InstitutionTitle = institutionTitle;
        Address = address;
        Types = types;
    }
}