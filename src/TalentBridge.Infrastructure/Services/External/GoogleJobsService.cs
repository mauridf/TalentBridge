using Microsoft.Extensions.Logging;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.Interfaces;

namespace TalentBridge.Infrastructure.Services.External;

public class GoogleJobsService : IGoogleJobsService
{
    private readonly ILogger<GoogleJobsService> _logger;

    public GoogleJobsService(ILogger<GoogleJobsService> logger)
    {
        _logger = logger;
    }

    public Task<ResultadoDto<bool>> PublicarVaga(Guid vagaId)
    {
        _logger.LogWarning("GoogleJobsService.PublicarVaga - Integração pendente. VagaId: {VagaId}", vagaId);
        return Task.FromResult(ResultadoDto<bool>.Ok(true));
    }

    public Task<ResultadoDto<bool>> RemoverVaga(Guid vagaId)
    {
        _logger.LogWarning("GoogleJobsService.RemoverVaga - Integração pendente. VagaId: {VagaId}", vagaId);
        return Task.FromResult(ResultadoDto<bool>.Ok(true));
    }
}
