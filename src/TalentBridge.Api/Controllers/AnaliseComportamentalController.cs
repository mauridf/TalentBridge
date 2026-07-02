using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.Interfaces;

namespace TalentBridge.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnaliseComportamentalController : ControllerBase
{
    private readonly IAnaliseComportamentalService _analiseService;
    private readonly ILogger<AnaliseComportamentalController> _logger;

    public AnaliseComportamentalController(
        IAnaliseComportamentalService analiseService,
        ILogger<AnaliseComportamentalController> logger)
    {
        _analiseService = analiseService;
        _logger = logger;
    }

    [HttpPost("Compatibilidade/{candidatoId}/{vagaId}")]
    [Authorize(Roles = "GESTOR_EMPRESA,RECRUTADOR")]
    public async Task<IActionResult> CalcularCompatibilidade(Guid candidatoId, Guid vagaId)
    {
        _logger.LogInformation("POST /api/AnaliseComportamental/Compatibilidade - Candidato: {CandidatoId}, Vaga: {VagaId}", candidatoId, vagaId);
        var resultado = await _analiseService.AnalisarCompatibilidade(candidatoId, vagaId);
        return resultado.Sucesso
            ? Ok(ResultadoDto<AnaliseBigFiveResponseDto>.Ok(resultado.Valor))
            : BadRequest(resultado);
    }

    [HttpPost("Relatorio/{candidatoId}")]
    [Authorize(Roles = "GESTOR_EMPRESA,RECRUTADOR")]
    public async Task<IActionResult> GerarRelatorio(Guid candidatoId)
    {
        _logger.LogInformation("POST /api/AnaliseComportamental/Relatorio - Candidato: {CandidatoId}", candidatoId);
        var resultado = await _analiseService.GerarRelatorioComportamental(candidatoId);
        return resultado.Sucesso
            ? Ok(ResultadoDto<AnaliseBigFiveResponseDto>.Ok(resultado.Valor))
            : BadRequest(resultado);
    }
}
