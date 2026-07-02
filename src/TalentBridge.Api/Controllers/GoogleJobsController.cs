using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.Interfaces;

namespace TalentBridge.Api.Controllers;

[ApiController]
[Route("api/admin/googlejobs")]
[Authorize(Roles = "ADMIN")]
public class GoogleJobsController : ControllerBase
{
    private readonly IGoogleJobsService _googleJobsService;
    private readonly ILogger<GoogleJobsController> _logger;

    public GoogleJobsController(IGoogleJobsService googleJobsService, ILogger<GoogleJobsController> logger)
    {
        _googleJobsService = googleJobsService;
        _logger = logger;
    }

    [HttpPost("publicar/{vagaId}")]
    public async Task<IActionResult> Publicar(Guid vagaId)
    {
        _logger.LogInformation("POST /api/admin/googlejobs/publicar/{VagaId}", vagaId);
        var resultado = await _googleJobsService.PublicarVaga(vagaId);
        return resultado.Sucesso
            ? Ok(new { mensagem = "Vaga publicada no Google Jobs com sucesso." })
            : BadRequest(ResultadoDto<object>.Falha("ERRO_PUBLICAR", "Erro ao publicar vaga no Google Jobs."));
    }

    [HttpPost("remover/{vagaId}")]
    public async Task<IActionResult> Remover(Guid vagaId)
    {
        _logger.LogInformation("POST /api/admin/googlejobs/remover/{VagaId}", vagaId);
        var resultado = await _googleJobsService.RemoverVaga(vagaId);
        return resultado.Sucesso
            ? Ok(new { mensagem = "Vaga removida do Google Jobs com sucesso." })
            : BadRequest(ResultadoDto<object>.Falha("ERRO_REMOVER", "Erro ao remover vaga do Google Jobs."));
    }
}
