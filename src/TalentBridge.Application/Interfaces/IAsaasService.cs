using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.DTOs.Credito;

namespace TalentBridge.Application.Interfaces;

public interface IAsaasService
{
    Task<ResultadoDto<CheckoutResponseDto>> RealizarCheckout(Guid pedidoId);
    Task<ResultadoDto<bool>> ProcessarWebhook(AsaasWebhookRequestDto webhook);
}
