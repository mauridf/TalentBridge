using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentBridge.Application.DTOs.Credito;
using TalentBridge.Application.Interfaces;

namespace TalentBridge.Api.Controllers;

/// <summary>
/// Controller para listagem de produtos
/// </summary>
[ApiController]
[Route("[controller]")]
public class ProdutoController : ControllerBase
{
    private readonly ICreditoService _creditoService;

    public ProdutoController(ICreditoService creditoService)
    {
        _creditoService = creditoService;
    }

    /// <summary>
    /// Lista todos os produtos disponíveis para compra
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Listar()
    {
        var resultado = await _creditoService.ListarProdutosAsync();
        return Ok(resultado.Value);
    }
}
