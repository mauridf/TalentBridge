using Microsoft.Extensions.Logging;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.DTOs.Credito;
using TalentBridge.Application.Interfaces;
using TalentBridge.Domain.Interfaces;

namespace TalentBridge.Application.Services;

public class AsaasService : IAsaasService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AsaasService> _logger;

    public AsaasService(IUnitOfWork unitOfWork, ILogger<AsaasService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public Task<ResultadoDto<CheckoutResponseDto>> RealizarCheckout(Guid pedidoId)
    {
        _logger.LogWarning("AsaasService.RealizarCheckout não implementado. PedidoId: {PedidoId}", pedidoId);
        throw new NotImplementedException("AsaasService.RealizarCheckout ainda não foi implementado.");
    }

    public Task<ResultadoDto<bool>> ProcessarWebhook(AsaasWebhookRequestDto webhook)
    {
        _logger.LogWarning("AsaasService.ProcessarWebhook não implementado.");
        throw new NotImplementedException("AsaasService.ProcessarWebhook ainda não foi implementado.");
    }
}
