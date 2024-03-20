using HiClass.Application.Dtos.UserDtos.Authentication;
using HiClass.Application.Models.User.Authentication;
using HiClass.Application.Tests.Common;

namespace HiClass.Infrastructure.Tests.Services;

public class UserAccountServiceTests : TestCommonBase
{
    public async Task LoginUser_ShouldLogIn()
    {
        //Arrange
        var requestUserDto = new UserLoginRequestDto
        {
            Email = "null",
            Password = null
        };
    }
}