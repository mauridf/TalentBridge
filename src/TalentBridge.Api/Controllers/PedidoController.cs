using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.DTOs.Credito;
using TalentBridge.Application.Interfaces;

namespace TalentBridge.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidoController : ControllerBase
{
    private readonly ICreditoService _creditoService;
    private readonly ILogger<PedidoController> _logger;

    public PedidoController(ICreditoService creditoService, ILogger<PedidoController> logger)
    {
        _creditoService = creditoService;
        _logger = logger;
    }

    [HttpPost("Listar")]
    [Authorize(Roles = "GESTOR_EMPRESA")]
    public async Task<IActionResult> Listar([FromBody] ListarPedidosRequestDto request)
    {
        _logger.LogInformation("POST /api/Pedido/Listar - CNPJ: {Cnpj}", request.Cnpj);
        var resultado = await _creditoService.ListarPedidosAsync(request.Cnpj);
        return resultado.IsSuccess
            ? Ok(ResultadoDto<IEnumerable<PedidoResponseDto>>.Ok(resultado.Value))
            : BadRequest(ResultadoDto<object>.Falha("ERRO_LISTAR", "Erro ao listar pedidos."));
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "GESTOR_EMPRESA")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var resultado = await _creditoService.ObterPedidoAsync(id);
        return resultado.IsSuccess
            ? Ok(ResultadoDto<PedidoResponseDto>.Ok(resultado.Value))
            : NotFound(ResultadoDto<object>.Falha("PEDIDO_NAO_ENCONTRADO", "Pedido não encontrado."));
    }
}
