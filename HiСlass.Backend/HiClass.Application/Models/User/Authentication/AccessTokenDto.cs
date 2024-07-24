using System.ComponentModel.DataAnnotations;

namespace HiClass.Application.Models.User.Authentication;

public class AccessTokenDto
{
    [Required] public string AccessToken { get; set; }

    public AccessTokenDto(string accessToken)
    {
        AccessToken = accessToken;
    }
}