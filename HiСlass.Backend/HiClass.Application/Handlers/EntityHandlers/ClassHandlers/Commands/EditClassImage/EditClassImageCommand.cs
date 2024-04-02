using System.ComponentModel.DataAnnotations;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.ClassHandlers.Commands.EditClassImage;

public class EditClassImageCommand : IRequest<string>
{
    [Required] public Guid ClassId { get; set; }

    [Required] public string ImageUrl { get; set; } = null!;
}