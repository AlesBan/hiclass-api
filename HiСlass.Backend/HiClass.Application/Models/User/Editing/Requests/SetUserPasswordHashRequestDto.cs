namespace HiClass.Application.Models.User.Editing.Requests;

public class SetUserPasswordHashRequestDto
{
    public string NewPassword { get; set; } = string.Empty;
}