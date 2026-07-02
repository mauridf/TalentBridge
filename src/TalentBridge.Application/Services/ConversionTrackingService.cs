using Microsoft.Extensions.Logging;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.Interfaces;

namespace TalentBridge.Application.Services;

public class ConversionTrackingService : IConversionTrackingService
{
    private readonly ILogger<ConversionTrackingService> _logger;

    public ConversionTrackingService(ILogger<ConversionTrackingService> logger)
    {
        _logger = logger;
    }

    public Task<ResultadoDto<bool>> RastrearConversaoAsync(RastrearConversaoRequestDto request)
    {
        _logger.LogInformation("Conversão registrada: {Tipo} de {Origem}", request.Tipo, request.Origem);
        return Task.FromResult(ResultadoDto<bool>.Ok(true));
    }
}
