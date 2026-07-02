using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.Interfaces;

namespace TalentBridge.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CupomController : ControllerBase
{
    private readonly ICupomService _cupomService;
    private readonly ILogger<CupomController> _logger;

    public CupomController(ICupomService cupomService, ILogger<CupomController> logger)
    {
        _cupomService = cupomService;
        _logger = logger;
    }

    [HttpPost]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Criar([FromBody] CriarCupomRequestDto request)
    {
        var resultado = await _cupomService.CriarCupom(request);
        return resultado.Sucesso
            ? Created(string.Empty, resultado)
            : BadRequest(resultado);
    }

    [HttpPost("Inativar/{id}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Inativar(Guid id)
    {
        var resultado = await _cupomService.Inativar(id);
        return resultado.Sucesso
            ? Ok(resultado)
            : BadRequest(resultado);
    }

    [HttpGet("{codigo}")]
    [AllowAnonymous]
    public async Task<IActionResult> BuscarPorCodigo(string codigo)
    {
        var resultado = await _cupomService.BuscarPorCodigo(codigo);
        return resultado.Sucesso
            ? Ok(resultado)
            : NotFound(resultado);
    }

    [HttpGet]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> ListarTodos()
    {
        var resultado = await _cupomService.ListarTodos();
        return Ok(resultado);
    }
}
