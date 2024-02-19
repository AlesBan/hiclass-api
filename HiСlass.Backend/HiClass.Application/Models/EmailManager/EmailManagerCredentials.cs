namespace HiClass.Application.Models.EmailManager;

public class EmailManagerCredentials
{
    public string Email { get; set; }
    public string Password { get; set; }

    public EmailManagerCredentials(string email, string password)
    {
        Email = email;
        Password = password;
    }
}