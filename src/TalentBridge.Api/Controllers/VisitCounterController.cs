using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.Interfaces;

namespace TalentBridge.Api.Controllers;

[ApiController]
[Route("api/VisitCounter")]
[AllowAnonymous]
public class VisitCounterController : ControllerBase
{
    private readonly IVisitCounterService _visitCounterService;
    private readonly ILogger<VisitCounterController> _logger;

    public VisitCounterController(IVisitCounterService visitCounterService, ILogger<VisitCounterController> logger)
    {
        _visitCounterService = visitCounterService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> RastrearVisita([FromBody] RastrearVisitaRequestDto request)
    {
        _logger.LogInformation("POST /api/VisitCounter");
        var resultado = await _visitCounterService.RastrearVisitaAsync(request);
        return resultado.Sucesso
            ? Ok(new { mensagem = "Visita registrada com sucesso." })
            : BadRequest(ResultadoDto<object>.Falha("ERRO_VISITA", "Erro ao registrar visita."));
    }
}
