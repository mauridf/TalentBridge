using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.Interfaces;

namespace TalentBridge.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompetenciaController : ControllerBase
{
    private readonly ICompetenciaService _competenciaService;
    private readonly ILogger<CompetenciaController> _logger;

    public CompetenciaController(ICompetenciaService competenciaService, ILogger<CompetenciaController> logger)
    {
        _competenciaService = competenciaService;
        _logger = logger;
    }

    [HttpPost]
    [Authorize(Roles = "GESTOR_EMPRESA")]
    public async Task<IActionResult> Criar([FromBody] CriarCompetenciaRequestDto request)
    {
        var resultado = await _competenciaService.CriarCompetencia(request);
        return resultado.Sucesso
            ? Created(string.Empty, resultado)
            : BadRequest(resultado);
    }

    [HttpPost("Buscar")]
    [AllowAnonymous]
    public async Task<IActionResult> Buscar([FromQuery] string? nome, [FromQuery] Guid? empresaId)
    {
        var resultado = await _competenciaService.Buscar(nome, empresaId);
        return Ok(resultado);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "GESTOR_EMPRESA")]
    public async Task<IActionResult> Remover(Guid id)
    {
        var resultado = await _competenciaService.Remover(id);
        return resultado.Sucesso
            ? Ok(resultado)
            : BadRequest(resultado);
    }
}
