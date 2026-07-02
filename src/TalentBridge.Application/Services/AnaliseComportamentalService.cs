using Microsoft.Extensions.Logging;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.Interfaces;
using TalentBridge.Domain.Interfaces;

namespace TalentBridge.Application.Services;

public class AnaliseComportamentalService : IAnaliseComportamentalService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AnaliseComportamentalService> _logger;

    public AnaliseComportamentalService(IUnitOfWork unitOfWork, ILogger<AnaliseComportamentalService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public Task<ResultadoDto<AnaliseBigFiveResponseDto>> AnalisarCompatibilidade(Guid candidatoId, Guid vagaId)
    {
        _logger.LogWarning("AnaliseComportamentalService.AnalisarCompatibilidade não implementado. CandidatoId: {CandidatoId}, VagaId: {VagaId}", candidatoId, vagaId);
        throw new NotImplementedException("AnaliseComportamentalService.AnalisarCompatibilidade ainda não foi implementado.");
    }

    public Task<ResultadoDto<AnaliseBigFiveResponseDto>> GerarRelatorioComportamental(Guid candidatoId)
    {
        _logger.LogWarning("AnaliseComportamentalService.GerarRelatorioComportamental não implementado. CandidatoId: {CandidatoId}", candidatoId);
        throw new NotImplementedException("AnaliseComportamentalService.GerarRelatorioComportamental ainda não foi implementado.");
    }
}
