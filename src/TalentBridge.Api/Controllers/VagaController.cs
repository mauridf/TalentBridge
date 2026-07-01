using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.DTOs.Vaga;
using TalentBridge.Application.Interfaces;

namespace TalentBridge.Api.Controllers;

/// <summary>
/// Controller responsável pela gestão de vagas
/// </summary>
[ApiController]
[Route("[controller]")]
public class VagaController : ControllerBase
{
    private readonly IVagaService _vagaService;
    private readonly ILogger<VagaController> _logger;

    public VagaController(IVagaService vagaService, ILogger<VagaController> logger)
    {
        _vagaService = vagaService;
        _logger = logger;
    }

    /// <summary>
    /// Cria ou edita uma vaga
    /// </summary>
    [HttpPost("upsert")]
    [Authorize]
    public async Task<IActionResult> Upsert([FromBody] VagaUpsertRequestDto request)
    {
        var empresaIdClaim = User.FindFirst("idEmpresa")?.Value;
        var usuarioIdClaim = User.FindFirst("id")?.Value;

        if (string.IsNullOrWhiteSpace(empresaIdClaim) || string.IsNullOrWhiteSpace(usuarioIdClaim))
            return Unauthorized(ResultadoDto<object>.Falha("SEM_EMPRESA", "Usuário não está associado a uma empresa."));

        var resultado = await _vagaService.UpsertAsync(
            Guid.Parse(empresaIdClaim),
            Guid.Parse(usuarioIdClaim),
            request);

        return resultado.IsSuccess
            ? Ok(ResultadoDto<VagaResponseDto>.Ok(resultado.Value))
            : BadRequest(ResultadoDto<object>.Falha(
                resultado.Errors.First().Message,
                resultado.Errors.First().Message));
    }

    /// <summary>
    /// Busca vagas ativas com filtros
    /// </summary>
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Buscar([FromBody] BuscarVagasRequestDto request)
    {
        var resultado = await _vagaService.BuscarVagasAsync(request);
        return Ok(resultado.Value);
    }

    /// <summary>
    /// Busca vaga por ID
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id)
    {
        var resultado = await _vagaService.GetByIdAsync(id);

        return resultado.IsSuccess
            ? Ok(ResultadoDto<VagaResponseDto>.Ok(resultado.Value))
            : NotFound(ResultadoDto<object>.Falha("VAGA_NAO_ENCONTRADA", "Vaga não encontrada."));
    }

    /// <summary>
    /// Lista vagas de uma empresa
    /// </summary>
    [HttpPost("empresa")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByEmpresa([FromBody] Guid empresaId)
    {
        var resultado = await _vagaService.GetByEmpresaAsync(empresaId);
        return Ok(ResultadoDto<IEnumerable<VagaResponseDto>>.Ok(resultado.Value));
    }

    /// <summary>
    /// Encerra uma vaga
    /// </summary>
    [HttpPost("encerrar")]
    [Authorize]
    public async Task<IActionResult> Encerrar([FromBody] Guid vagaId)
    {
        var usuarioIdClaim = User.FindFirst("id")?.Value;
        if (string.IsNullOrWhiteSpace(usuarioIdClaim))
            return Unauthorized();

        var resultado = await _vagaService.EncerrarAsync(vagaId, Guid.Parse(usuarioIdClaim));

        return resultado.IsSuccess
            ? Ok(new { mensagem = "Vaga encerrada com sucesso." })
            : BadRequest(ResultadoDto<object>.Falha("ERRO_ENCERRAR", "Erro ao encerrar vaga."));
    }

    /// <summary>
    /// Reativa uma vaga
    /// </summary>
    [HttpPost("reativar")]
    [Authorize]
    public async Task<IActionResult> Reativar([FromBody] Guid vagaId)
    {
        var resultado = await _vagaService.ReativarAsync(vagaId);

        return resultado.IsSuccess
            ? Ok(new { mensagem = "Vaga reativada com sucesso." })
            : BadRequest(ResultadoDto<object>.Falha("ERRO_REATIVAR", "Erro ao reativar vaga."));
    }
}
