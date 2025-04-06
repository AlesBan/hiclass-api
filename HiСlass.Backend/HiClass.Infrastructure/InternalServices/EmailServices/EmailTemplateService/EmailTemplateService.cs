// EmailTemplateService.cs

using System.Collections.Concurrent;
using System.Reflection;

namespace HiClass.Infrastructure.InternalServices.EmailServices.EmailTemplateService;

public class EmailTemplateService : IEmailTemplateService
{
    private static readonly ConcurrentDictionary<string, string> _templateCache = new();

    public string LoadTemplate(string templateName, Dictionary<string, string> replacements)
    {
        var templateContent = GetTemplateContent(templateName);
        return ReplacePlaceholders(templateContent, replacements);
    }

    public async Task<string> LoadTemplateAsync(string templateName, Dictionary<string, string> replacements)
    {
        var templateContent = await GetTemplateContentAsync(templateName);
        return ReplacePlaceholders(templateContent, replacements);
    }

    private static string GetTemplateContent(string templateName)
    {
        if (_templateCache.TryGetValue(templateName, out var templateContent))
        {
            return templateContent;
        }
        var templatePath = Path.Combine("Resources", "EmailTemplates", $"{templateName}.html");
        templateContent = File.ReadAllText(templatePath);
        _templateCache[templateName] = templateContent;
        return templateContent;
    }

    private async Task<string> GetTemplateContentAsync(string templateName)
    {
        if (!_templateCache.TryGetValue(templateName, out var templateContent))
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"HiClass.Infrastructure.InternalServices.EmailServices.EmailTemplateService.Resources.EmailTemplates.{templateName}.html";
        
            await using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                throw new FileNotFoundException($"Embedded resource {resourceName} not found");
            }
        
            using var reader = new StreamReader(stream);
            templateContent = await reader.ReadToEndAsync();
            _templateCache[templateName] = templateContent;
        }
        return templateContent;
    }
    private static string ReplacePlaceholders(string content, Dictionary<string, string> replacements)
    {
        foreach (var replacement in replacements)
        {
            content = content
                .Replace($"*|{replacement.Key}|*", replacement.Value)
                .Replace($"{{{replacement.Key}}}", replacement.Value);
        }
        return content;
    }
}