using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.Interfaces;

namespace TalentBridge.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WhatsAppController : ControllerBase
{
    private readonly IWhatsAppService _whatsAppService;
    private readonly ILogger<WhatsAppController> _logger;

    public WhatsAppController(IWhatsAppService whatsAppService, ILogger<WhatsAppController> logger)
    {
        _whatsAppService = whatsAppService;
        _logger = logger;
    }

    [HttpPost("Enviar")]
    [AllowAnonymous]
    public async Task<IActionResult> EnviarMensagem([FromQuery] string telefone, [FromQuery] string mensagem)
    {
        _logger.LogInformation("POST /api/WhatsApp/Enviar - Telefone: {Telefone}", telefone);
        var resultado = await _whatsAppService.EnviarMensagem(telefone, mensagem);
        return resultado.Sucesso
            ? Ok(ResultadoDto<string>.Ok(resultado.Valor))
            : BadRequest(ResultadoDto<object>.Falha("ERRO_ENVIAR", "Erro ao enviar mensagem WhatsApp."));
    }

    [HttpPost("Candidatar")]
    [AllowAnonymous]
    public async Task<IActionResult> Candidatar([FromQuery] Guid vagaId, [FromQuery] string telefone, [FromQuery] string nome)
    {
        _logger.LogInformation("POST /api/WhatsApp/Candidatar - Vaga: {VagaId}", vagaId);
        var resultado = await _whatsAppService.CandidatarViaWhatsApp(vagaId, telefone, nome);
        return resultado.Sucesso
            ? Ok(new { mensagem = "Candidatura realizada com sucesso." })
            : BadRequest(ResultadoDto<object>.Falha("ERRO_CANDIDATAR", "Erro ao candidatar via WhatsApp."));
    }
}
