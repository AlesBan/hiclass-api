using System.ComponentModel.DataAnnotations;
using HiClass.Application.Attributes;

namespace HiClass.Application.Models.User.Editing.Requests;

public class EditUserPasswordHashRequestDto
{
    [Required][NotEqual(nameof(NewPassword), nameof(String))]
    public string OldPassword { get; set; } = string.Empty;
    [Required] public string NewPassword { get; set; } = string.Empty;
}