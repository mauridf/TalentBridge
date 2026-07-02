using Microsoft.Extensions.Logging;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.Interfaces;

namespace TalentBridge.Infrastructure.Services.External;

public class WhatsAppIntegrationService : IWhatsAppService
{
    private readonly ILogger<WhatsAppIntegrationService> _logger;

    public WhatsAppIntegrationService(ILogger<WhatsAppIntegrationService> logger)
    {
        _logger = logger;
    }

    public Task<ResultadoDto<string>> EnviarMensagem(string telefone, string mensagem)
    {
        _logger.LogWarning("WhatsAppIntegrationService.EnviarMensagem - Integração pendente. Telefone: {Telefone}, Mensagem: {Mensagem}", telefone, mensagem);
        return Task.FromResult(ResultadoDto<string>.Ok($"Mensagem registrada para envio ao {telefone}."));
    }

    public Task<ResultadoDto<bool>> CandidatarViaWhatsApp(Guid vagaId, string telefone, string nome)
    {
        _logger.LogWarning("WhatsAppIntegrationService.CandidatarViaWhatsApp - Integração pendente. VagaId: {VagaId}, Telefone: {Telefone}, Nome: {Nome}", vagaId, telefone, nome);
        return Task.FromResult(ResultadoDto<bool>.Ok(true));
    }
}
