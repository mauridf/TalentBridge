using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.Interfaces;

namespace TalentBridge.Infrastructure.Services.Email;

public class SmtpSettings
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FromEmail { get; set; } = string.Empty;
    public string FromName { get; set; } = string.Empty;
}

public class EmailService : IEmailService
{
    private readonly SmtpSettings _smtpSettings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<SmtpSettings> smtpSettings, ILogger<EmailService> logger)
    {
        _smtpSettings = smtpSettings.Value;
        _logger = logger;
    }

    public async Task<ResultadoDto<bool>> SendEmailAsync(string to, string subject, string body)
    {
        try
        {
            using var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
            {
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                EnableSsl = _smtpSettings.Port == 587
            };

            var mail = new MailMessage
            {
                From = new MailAddress(_smtpSettings.FromEmail, _smtpSettings.FromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mail.To.Add(to);

            await client.SendMailAsync(mail);

            _logger.LogInformation("Email enviado com sucesso para {To}", to);
            return ResultadoDto<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao enviar email para {To}", to);
            return ResultadoDto<bool>.Falha("EMAIL_ERROR", ex.Message);
        }
    }

    public async Task<ResultadoDto<bool>> SendEmailWithTemplateAsync(string to, string subject, string templateName, Dictionary<string, string> placeholders)
    {
        try
        {
            var templatePath = Path.Combine("Templates", "Email", $"{templateName}.html");

            if (!File.Exists(templatePath))
            {
                _logger.LogWarning("Template de email não encontrado: {TemplatePath}", templatePath);
                return ResultadoDto<bool>.Falha("TEMPLATE_NOT_FOUND", $"Template '{templateName}' não encontrado.");
            }

            var body = await File.ReadAllTextAsync(templatePath);

            foreach (var placeholder in placeholders)
                body = body.Replace($"{{{{{placeholder.Key}}}}}", placeholder.Value);

            return await SendEmailAsync(to, subject, body);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao enviar email com template para {To}", to);
            return ResultadoDto<bool>.Falha("EMAIL_TEMPLATE_ERROR", ex.Message);
        }
    }
}
