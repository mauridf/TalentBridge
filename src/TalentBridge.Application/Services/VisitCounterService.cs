using Microsoft.Extensions.Logging;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.Interfaces;

namespace TalentBridge.Application.Services;

public class VisitCounterService : IVisitCounterService
{
    private readonly ILogger<VisitCounterService> _logger;

    public VisitCounterService(ILogger<VisitCounterService> logger)
    {
        _logger = logger;
    }

    public Task<ResultadoDto<bool>> RastrearVisitaAsync(RastrearVisitaRequestDto request)
    {
        _logger.LogInformation("Visita registrada: {Pagina} de {Origem}", request.Pagina, request.Origem);
        return Task.FromResult(ResultadoDto<bool>.Ok(true));
    }
}
