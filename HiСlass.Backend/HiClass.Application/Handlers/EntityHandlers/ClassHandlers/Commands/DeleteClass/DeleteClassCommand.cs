using System.ComponentModel.DataAnnotations;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.ClassHandlers.Commands.DeleteClass;

public class DeleteClassCommand : IRequest<Unit>
{
    [Required] public Guid UserOwnerId { get; set; }
    [Required] public Guid ClassId { get; set; }

}