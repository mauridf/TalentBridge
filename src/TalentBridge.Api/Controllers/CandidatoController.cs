using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentBridge.Application.DTOs.Candidato;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.Interfaces;

namespace TalentBridge.Api.Controllers;

/// <summary>
/// Controller responsável pela gestão de candidatos
/// </summary>
[ApiController]
[Route("[controller]")]
public class CandidatoController : ControllerBase
{
    private readonly ICandidatoService _candidatoService;
    private readonly ILogger<CandidatoController> _logger;

    public CandidatoController(ICandidatoService candidatoService, ILogger<CandidatoController> logger)
    {
        _candidatoService = candidatoService;
        _logger = logger;
    }

    /// <summary>
    /// Cria um novo candidato (registro público)
    /// </summary>
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Criar([FromBody] CriarCandidatoRequestDto request)
    {
        _logger.LogInformation("POST /Candidato - Criar candidato: {Email}", request.Email);

        var resultado = await _candidatoService.CriarAsync(request);

        if (resultado.IsSuccess)
            return CreatedAtAction(nameof(GetById), new { id = resultado.Value.Id },
                ResultadoDto<CriarCandidatoResponseDto>.Ok(resultado.Value));

        var erro = resultado.Errors.First();
        return BadRequest(ResultadoDto<object>.Falha(erro.Message, erro.Message));
    }

    /// <summary>
    /// Busca candidato por ID ou email
    /// </summary>
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetById([FromQuery] Guid? id, [FromQuery] string? email)
    {
        if (id.HasValue)
        {
            var resultado = await _candidatoService.GetByIdAsync(id.Value);
            return resultado.IsSuccess
                ? Ok(ResultadoDto<CandidatoResponseDto>.Ok(resultado.Value))
                : NotFound(ResultadoDto<object>.Falha("CANDIDATO_NAO_ENCONTRADO", "Candidato não encontrado."));
        }

        if (!string.IsNullOrWhiteSpace(email))
        {
            var resultado = await _candidatoService.GetByEmailAsync(email);
            return resultado.IsSuccess
                ? Ok(ResultadoDto<CandidatoResponseDto>.Ok(resultado.Value))
                : NotFound(ResultadoDto<object>.Falha("CANDIDATO_NAO_ENCONTRADO", "Candidato não encontrado."));
        }

        return BadRequest(ResultadoDto<object>.Falha("PARAMETRO_INVALIDO", "Informe id ou email."));
    }

    /// <summary>
    /// Edita dados do candidato
    /// </summary>
    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Editar([FromBody] EditarCandidatoRequestDto request)
    {
        var usuarioIdClaim = User.FindFirst("id")?.Value;
        if (string.IsNullOrWhiteSpace(usuarioIdClaim))
            return Unauthorized();

        var usuarioId = Guid.Parse(usuarioIdClaim);
        var resultado = await _candidatoService.EditarAsync(usuarioId, request);

        return resultado.IsSuccess
            ? Ok(ResultadoDto<CandidatoResponseDto>.Ok(resultado.Value))
            : BadRequest(ResultadoDto<object>.Falha("ERRO_EDICAO", "Erro ao editar candidato."));
    }

    /// <summary>
    /// Confirma email do candidato (via body)
    /// </summary>
    [HttpPost("confirmarEmail")]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmarEmail([FromBody] ConfirmarEmailRequestDto request)
    {
        var resultado = await _candidatoService.ConfirmarEmailAsync(request);

        return resultado.IsSuccess
            ? Ok(new { mensagem = "Email confirmado com sucesso!" })
            : BadRequest(ResultadoDto<object>.Falha("TOKEN_INVALIDO", "Token de confirmação inválido."));
    }

    /// <summary>
    /// Reenvia email de confirmação
    /// </summary>
    [HttpPost("reenviarConfirmacaoEmail")]
    [AllowAnonymous]
    public async Task<IActionResult> ReenviarConfirmacaoEmail([FromBody] ReenviarConfirmacaoEmailRequestDto request)
    {
        var resultado = await _candidatoService.ReenviarConfirmacaoEmailAsync(request.Email);

        return Ok(new { mensagem = "Se o email existir, um novo link de confirmação será enviado." });
    }
}
