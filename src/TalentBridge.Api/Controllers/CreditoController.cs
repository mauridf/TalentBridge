using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.DTOs.Credito;
using TalentBridge.Application.Interfaces;

namespace TalentBridge.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CreditoController : ControllerBase
{
    private readonly ICreditoService _creditoService;
    private readonly ILogger<CreditoController> _logger;

    public CreditoController(ICreditoService creditoService, ILogger<CreditoController> logger)
    {
        _creditoService = creditoService;
        _logger = logger;
    }

    [HttpGet("empresa/{empresaId}")]
    [Authorize(Roles = "GESTOR_EMPRESA")]
    public async Task<IActionResult> SaldoEmpresa(Guid empresaId)
    {
        var resultado = await _creditoService.ObterCreditosAsync(empresaId);
        return resultado.IsSuccess
            ? Ok(ResultadoDto<CreditosResponseDto>.Ok(resultado.Value))
            : NotFound(ResultadoDto<object>.Falha("CREDITOS_NAO_ENCONTRADOS", "Saldo não encontrado para a empresa."));
    }

    [HttpPost("Admin/Adicionar")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> AdminAdicionar(Guid empresaId, int quantidade, string? motivo = null)
    {
        _logger.LogInformation("POST /api/Credito/Admin/Adicionar");
        return Ok(new { mensagem = "Funcionalidade em desenvolvimento." });
    }

    [HttpPost("Admin/Remover")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> AdminRemover(Guid empresaId, int quantidade, string? motivo = null)
    {
        _logger.LogInformation("POST /api/Credito/Admin/Remover");
        return Ok(new { mensagem = "Funcionalidade em desenvolvimento." });
    }
}
