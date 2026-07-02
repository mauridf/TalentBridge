using TalentBridge.Application.DTOs.Common;

namespace TalentBridge.Application.Interfaces;

public interface IWhatsAppService
{
    Task<ResultadoDto<string>> EnviarMensagem(string telefone, string mensagem);
    Task<ResultadoDto<bool>> CandidatarViaWhatsApp(Guid vagaId, string telefone, string nome);
}
