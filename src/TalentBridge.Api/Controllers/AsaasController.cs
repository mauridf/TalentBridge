using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.DTOs.Credito;
using TalentBridge.Application.Interfaces;

namespace TalentBridge.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AsaasController : ControllerBase
{
    private readonly ICreditoService _creditoService;
    private readonly ILogger<AsaasController> _logger;

    public AsaasController(ICreditoService creditoService, ILogger<AsaasController> logger)
    {
        _creditoService = creditoService;
        _logger = logger;
    }

    [HttpPost("RealizarCheckout")]
    [Authorize(Roles = "GESTOR_EMPRESA")]
    public async Task<IActionResult> RealizarCheckout(Guid pedidoId)
    {
        var resultado = await _creditoService.RealizarCheckoutAsync(pedidoId);
        return resultado.IsSuccess
            ? Ok(ResultadoDto<CheckoutResponseDto>.Ok(resultado.Value))
            : BadRequest(ResultadoDto<object>.Falha("ERRO_CHECKOUT", "Erro ao realizar checkout."));
    }

    [HttpPost("Webhook")]
    [AllowAnonymous]
    public async Task<IActionResult> Webhook([FromBody] AsaasWebhookRequestDto request)
    {
        var resultado = await _creditoService.ProcessarWebhookAsync(request);
        return resultado.IsSuccess
            ? Ok(new { mensagem = "Webhook processado com sucesso." })
            : BadRequest(ResultadoDto<object>.Falha("ERRO_WEBHOOK", "Erro ao processar webhook."));
    }
}
