namespace HiClass.Application.Interfaces.Services.ExternalServices;

public interface IYandexAccountService
{
    public string GeyAccessToken();
    public string GetBaseUrl();
}