using TalentBridge.Application.DTOs.Common;

namespace TalentBridge.Application.Interfaces;

public interface IEmailService
{
    Task<ResultadoDto<bool>> SendEmailAsync(string to, string subject, string body);
    Task<ResultadoDto<bool>> SendEmailWithTemplateAsync(string to, string subject, string templateName, Dictionary<string, string> placeholders);
}
