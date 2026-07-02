using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.Interfaces;

namespace TalentBridge.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParametrosGeraisController : ControllerBase
{
    private readonly IParametrosGeraisService _parametrosGeraisService;
    private readonly ILogger<ParametrosGeraisController> _logger;

    public ParametrosGeraisController(
        IParametrosGeraisService parametrosGeraisService,
        ILogger<ParametrosGeraisController> logger)
    {
        _parametrosGeraisService = parametrosGeraisService;
        _logger = logger;
    }

    [HttpGet("{chave}")]
    [AllowAnonymous]
    public async Task<IActionResult> ObterPorChave(string chave)
    {
        var resultado = await _parametrosGeraisService.Obter(chave);
        return resultado.Sucesso
            ? Ok(ResultadoDto<ParametroResponseDto>.Ok(resultado.Valor))
            : NotFound(ResultadoDto<object>.Falha("PARAMETRO_NAO_ENCONTRADO", "Parâmetro não encontrado."));
    }

    [HttpGet]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> ListarTodos()
    {
        var resultado = await _parametrosGeraisService.ListarTodos();
        return Ok(ResultadoDto<List<ParametroResponseDto>>.Ok(resultado.Valor));
    }

    [HttpPut("{chave}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Atualizar(string chave, [FromBody] AtualizarParametroRequestDto request)
    {
        _logger.LogInformation("PUT /api/ParametrosGerais/{Chave}", chave);
        var resultado = await _parametrosGeraisService.Atualizar(chave, request.Valor);
        return resultado.Sucesso
            ? Ok(new { mensagem = "Parâmetro atualizado com sucesso." })
            : BadRequest(ResultadoDto<object>.Falha("ERRO_ATUALIZAR", "Erro ao atualizar parâmetro."));
    }
}
