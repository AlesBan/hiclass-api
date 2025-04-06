namespace HiClass.Infrastructure.InternalServices.EmailServices.EmailTemplateService;

public interface IEmailTemplateService
{
    string LoadTemplate(string templateName, Dictionary<string, string> replacements);
    Task<string> LoadTemplateAsync(string templateName, Dictionary<string, string> replacements);
}