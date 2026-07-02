using Microsoft.Extensions.Logging;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.Interfaces;

namespace TalentBridge.Application.Services;

public class WhatsAppService : IWhatsAppService
{
    private readonly ILogger<WhatsAppService> _logger;

    public WhatsAppService(ILogger<WhatsAppService> logger)
    {
        _logger = logger;
    }

    public Task<ResultadoDto<string>> EnviarMensagem(string telefone, string mensagem)
    {
        _logger.LogWarning("WhatsAppService.EnviarMensagem não implementado. Telefone: {Telefone}", telefone);
        throw new NotImplementedException("WhatsAppService.EnviarMensagem ainda não foi implementado.");
    }

    public Task<ResultadoDto<bool>> CandidatarViaWhatsApp(Guid vagaId, string telefone, string nome)
    {
        _logger.LogWarning("WhatsAppService.CandidatarViaWhatsApp não implementado. VagaId: {VagaId}, Telefone: {Telefone}", vagaId, telefone);
        throw new NotImplementedException("WhatsAppService.CandidatarViaWhatsApp ainda não foi implementado.");
    }
}
