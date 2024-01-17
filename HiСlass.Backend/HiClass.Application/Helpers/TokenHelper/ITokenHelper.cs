using HiClass.Domain.Entities.Main;

namespace HiClass.Application.Helpers.TokenHelper;

public interface ITokenHelper
{
    public string CreateToken(User user);
}