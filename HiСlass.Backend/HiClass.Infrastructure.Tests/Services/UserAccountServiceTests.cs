using HiClass.Application.Models.User.Authentication;
using HiClass.Application.Tests.Common;

namespace HiClass.Infrastructure.Tests.Services;

public class UserAccountServiceTests : TestCommonBase
{
    public async Task LoginUser_ShouldLogIn()
    {
        //Arrange
        var requestUserDto = new LoginRequestDto
        {
            Email = "null",
            Password = null
        };
    }
}