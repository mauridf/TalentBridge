using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentBridge.Application.DTOs.Candidatura;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.Interfaces;

namespace TalentBridge.Api.Controllers;

/// <summary>
/// Controller responsável pela gestão de candidaturas
/// </summary>
[ApiController]
[Route("[controller]")]
public class CandidaturaController : ControllerBase
{
    private readonly ICandidaturaService _candidaturaService;
    private readonly ILogger<CandidaturaController> _logger;

    public CandidaturaController(
        ICandidaturaService candidaturaService,
        ILogger<CandidaturaController> logger)
    {
        _candidaturaService = candidaturaService;
        _logger = logger;
    }

    /// <summary>
    /// Cria uma nova candidatura (candidato se candidata a uma vaga)
    /// </summary>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Criar([FromBody] CriarCandidaturaRequestDto request)
    {
        var usuarioIdClaim = User.FindFirst("id")?.Value;
        if (string.IsNullOrWhiteSpace(usuarioIdClaim))
            return Unauthorized();

        var resultado = await _candidaturaService.CriarAsync(
            Guid.Parse(usuarioIdClaim), request);

        if (resultado.IsSuccess)
            return Created(string.Empty, ResultadoDto<CandidaturaResponseDto>.Ok(resultado.Value));

        var erro = resultado.Errors.First();
        return BadRequest(ResultadoDto<object>.Falha(erro.Message, ObterMensagemErro(erro.Message)));
    }

    /// <summary>
    /// Verifica se já existe candidatura para uma vaga
    /// </summary>
    [HttpPost("verificar")]
    [Authorize]
    public async Task<IActionResult> VerificarCandidatura([FromBody] CriarCandidaturaRequestDto request)
    {
        var usuarioIdClaim = User.FindFirst("id")?.Value;
        if (string.IsNullOrWhiteSpace(usuarioIdClaim))
            return Unauthorized();

        var resultado = await _candidaturaService.VerificarCandidaturaExistenteAsync(
            request.VagaId, Guid.Parse(usuarioIdClaim));

        return Ok(ResultadoDto<bool>.Ok(resultado.Value));
    }

    /// <summary>
    /// Busca candidaturas com filtros (para gestores e recrutadores)
    /// </summary>
    [HttpPost("Buscar")]
    [Authorize]
    public async Task<IActionResult> Buscar([FromBody] BuscarCandidaturasRequestDto request)
    {
        var resultado = await _candidaturaService.BuscarAsync(request);

        return resultado.IsSuccess
            ? Ok(ResultadoDto<IEnumerable<CandidaturaResponseDto>>.Ok(resultado.Value))
            : BadRequest(ResultadoDto<object>.Falha("ERRO_BUSCA", "Erro ao buscar candidaturas."));
    }

    /// <summary>
    /// Lista candidaturas de uma vaga específica
    /// </summary>
    [HttpGet("vaga/{vagaId}")]
    [Authorize]
    public async Task<IActionResult> GetByVaga(Guid vagaId)
    {
        var resultado = await _candidaturaService.GetByVagaAsync(vagaId);
        return Ok(ResultadoDto<IEnumerable<CandidaturaResponseDto>>.Ok(resultado.Value));
    }

    /// <summary>
    /// Lista candidaturas do candidato logado
    /// </summary>
    [HttpGet("minhas")]
    [Authorize]
    public async Task<IActionResult> GetMinhasCandidaturas()
    {
        var usuarioIdClaim = User.FindFirst("id")?.Value;
        if (string.IsNullOrWhiteSpace(usuarioIdClaim))
            return Unauthorized();

        var resultado = await _candidaturaService.GetByCandidatoAsync(Guid.Parse(usuarioIdClaim));
        return Ok(ResultadoDto<IEnumerable<CandidaturaResponseDto>>.Ok(resultado.Value));
    }

    /// <summary>
    /// Agenda uma entrevista para um candidato
    /// </summary>
    [HttpPost("Entrevista")]
    [Authorize]
    public async Task<IActionResult> AgendarEntrevista([FromBody] AgendarEntrevistaRequestDto request)
    {
        var resultado = await _candidaturaService.AgendarEntrevistaAsync(request);

        return resultado.IsSuccess
            ? Ok(new { mensagem = "Entrevista agendada com sucesso!" })
            : BadRequest(ResultadoDto<object>.Falha("ERRO_AGENDAR", "Erro ao agendar entrevista."));
    }

    /// <summary>
    /// Marca entrevista como realizada
    /// </summary>
    [HttpPost("{id}/entrevista-realizada")]
    [Authorize]
    public async Task<IActionResult> MarcarEntrevistaRealizada(Guid id)
    {
        var resultado = await _candidaturaService.MarcarEntrevistaRealizadaAsync(id);

        return resultado.IsSuccess
            ? Ok(new { mensagem = "Entrevista marcada como realizada." })
            : BadRequest(ResultadoDto<object>.Falha("ERRO_ATUALIZAR", "Erro ao atualizar candidatura."));
    }

    /// <summary>
    /// Marca candidato como contratado
    /// </summary>
    [HttpPost("Contratar")]
    [Authorize]
    public async Task<IActionResult> Contratar([FromBody] ContratarRequestDto request)
    {
        var resultado = await _candidaturaService.ContratarAsync(request.CandidaturaId);

        return resultado.IsSuccess
            ? Ok(new { mensagem = "Candidato contratado com sucesso!" })
            : BadRequest(ResultadoDto<object>.Falha("ERRO_CONTRATAR", "Erro ao contratar candidato."));
    }

    /// <summary>
    /// Obtém mensagem amigável para o código de erro
    /// </summary>
    private static string ObterMensagemErro(string codigo) => codigo switch
    {
        "VAGA_NAO_ENCONTRADA" => "Vaga não encontrada.",
        "VAGA_FORA_PERIODO_CANDIDATURA" => "Esta vaga não está mais aceitando candidaturas.",
        "CANDIDATURA_JA_EXISTENTE" => "Você já se candidatou para esta vaga.",
        "CANDIDATO_NAO_ENCONTRADO" => "Candidato não encontrado.",
        _ => "Erro ao processar candidatura."
    };
}
