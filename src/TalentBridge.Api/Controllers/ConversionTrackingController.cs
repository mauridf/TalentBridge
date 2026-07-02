using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.Interfaces;

namespace TalentBridge.Api.Controllers;

[ApiController]
[Route("api/ConversionTracking")]
[AllowAnonymous]
public class ConversionTrackingController : ControllerBase
{
    private readonly IConversionTrackingService _conversionTrackingService;
    private readonly ILogger<ConversionTrackingController> _logger;

    public ConversionTrackingController(
        IConversionTrackingService conversionTrackingService,
        ILogger<ConversionTrackingController> logger)
    {
        _conversionTrackingService = conversionTrackingService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> RastrearConversao([FromBody] RastrearConversaoRequestDto request)
    {
        _logger.LogInformation("POST /api/ConversionTracking");
        var resultado = await _conversionTrackingService.RastrearConversaoAsync(request);
        return resultado.Sucesso
            ? Ok(new { mensagem = "Conversão registrada com sucesso." })
            : BadRequest(ResultadoDto<object>.Falha("ERRO_CONVERSAO", "Erro ao registrar conversão."));
    }
}
