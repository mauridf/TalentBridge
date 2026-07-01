using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentBridge.Application.DTOs.Credito;
using TalentBridge.Application.Interfaces;

namespace TalentBridge.Api.Controllers;

/// <summary>
/// Controller para gestão do carrinho de compras
/// </summary>
[ApiController]
[Route("[controller]")]
[Authorize]
public class CarrinhoController : ControllerBase
{
    private readonly ICreditoService _creditoService;

    public CarrinhoController(ICreditoService creditoService)
    {
        _creditoService = creditoService;
    }

    /// <summary>
    /// Obtém o carrinho ativo da empresa
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ObterCarrinho()
    {
        var empresaId = ObterEmpresaId();
        if (empresaId == null) return Unauthorized();

        var resultado = await _creditoService.ObterCarrinhoAsync(empresaId.Value);
        return Ok(resultado.Value);
    }

    /// <summary>
    /// Adiciona/atualiza item no carrinho
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> AtualizarCarrinho([FromBody] AtualizarCarrinhoRequestDto request)
    {
        var empresaId = ObterEmpresaId();
        if (empresaId == null) return Unauthorized();

        var resultado = await _creditoService.AtualizarCarrinhoAsync(empresaId.Value, request);

        return resultado.IsSuccess
            ? Ok(resultado.Value)
            : BadRequest(new { erro = resultado.Errors.First().Message });
    }

    /// <summary>
    /// Finaliza o carrinho e gera pedido
    /// </summary>
    [HttpPost("finalizar")]
    public async Task<IActionResult> FinalizarCarrinho()
    {
        var empresaId = ObterEmpresaId();
        if (empresaId == null) return Unauthorized();

        var resultado = await _creditoService.FinalizarCarrinhoAsync(empresaId.Value);

        return resultado.IsSuccess
            ? Ok(resultado.Value)
            : BadRequest(new { erro = resultado.Errors.First().Message });
    }

    private Guid? ObterEmpresaId()
    {
        var empresaIdClaim = User.FindFirst("idEmpresa")?.Value;
        return Guid.TryParse(empresaIdClaim, out var id) ? id : null;
    }
}
