namespace HiClass.Application.Models.User.Editing.Requests;

public class EditUserPasswordHashRequestDto
{
    public string OldPassword { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}