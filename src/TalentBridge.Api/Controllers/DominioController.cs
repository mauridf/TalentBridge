using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.Interfaces;
using TalentBridge.Domain.Enums;

namespace TalentBridge.Api.Controllers;

/// <summary>
/// Controller para consulta de domínios (lookup table)
/// </summary>
[ApiController]
[Route("[controller]")]
[AllowAnonymous]
public class DominioController : ControllerBase
{
    private readonly IDominioService _dominioService;

    public DominioController(IDominioService dominioService)
    {
        _dominioService = dominioService;
    }

    /// <summary>
    /// Busca domínios por tipo
    /// </summary>
    [HttpGet("{tipo}")]
    public async Task<IActionResult> BuscarPorTipo(TipoDominioEnum tipo)
    {
        var resultado = await _dominioService.BuscarPorTipoAsync(tipo);
        return resultado.IsSuccess
            ? Ok(ResultadoDto<IEnumerable<DominioDto>>.Ok(resultado.Value))
            : BadRequest(ResultadoDto<object>.Falha("ERRO_DOMINIO", "Erro ao buscar domínios."));
    }

    /// <summary>
    /// Busca todos os domínios agrupados
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> BuscarTodos()
    {
        var resultado = await _dominioService.BuscarTodosAsync();
        return resultado.IsSuccess
            ? Ok(ResultadoDto<Dictionary<string, List<DominioDto>>>.Ok(resultado.Value))
            : BadRequest(ResultadoDto<object>.Falha("ERRO_DOMINIO", "Erro ao buscar domínios."));
    }
}
