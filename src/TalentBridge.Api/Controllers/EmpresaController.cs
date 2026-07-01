using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.DTOs.Empresa;
using TalentBridge.Application.DTOs.Usuario;
using TalentBridge.Application.Interfaces;

namespace TalentBridge.Api.Controllers;

/// <summary>
/// Controller responsável pela gestão de empresas
/// </summary>
[ApiController]
[Route("[controller]")]
public class EmpresaController : ControllerBase
{
    private readonly IEmpresaService _empresaService;
    private readonly ILogger<EmpresaController> _logger;

    public EmpresaController(IEmpresaService empresaService, ILogger<EmpresaController> logger)
    {
        _empresaService = empresaService;
        _logger = logger;
    }

    /// <summary>
    /// Cria uma empresa com gestor (via convite)
    /// </summary>
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Criar([FromBody] CriarEmpresaRequestDto request)
    {
        _logger.LogInformation("POST /Empresa - Criar empresa: {Email}", request.EmailGestor);

        var resultado = await _empresaService.CriarEmpresaComGestorAsync(request);

        if (resultado.IsSuccess)
            return Created(string.Empty, ResultadoDto<CriarEmpresaResponseDto>.Ok(resultado.Value));

        var erro = resultado.Errors.First();
        return BadRequest(ResultadoDto<object>.Falha(erro.Message, erro.Message));
    }

    /// <summary>
    /// Cria um convite para empresa ou recrutador
    /// </summary>
    [HttpPost("convite")]
    [Authorize]
    public async Task<IActionResult> CriarConvite([FromBody] CriarConviteRequestDto request)
    {
        var empresaIdClaim = User.FindFirst("idEmpresa")?.Value;
        if (string.IsNullOrWhiteSpace(empresaIdClaim))
            return Unauthorized(ResultadoDto<object>.Falha("SEM_EMPRESA", "Usuário não está associado a uma empresa."));

        var empresaId = Guid.Parse(empresaIdClaim);
        var resultado = await _empresaService.CriarConviteAsync(empresaId, request);

        return resultado.IsSuccess
            ? Created(string.Empty, ResultadoDto<ConviteResponseDto>.Ok(resultado.Value))
            : BadRequest(ResultadoDto<object>.Falha("ERRO_CONVITE", "Erro ao criar convite."));
    }

    /// <summary>
    /// Valida um token de convite
    /// </summary>
    [HttpGet("convite/validar/{token}")]
    [AllowAnonymous]
    public async Task<IActionResult> ValidarConvite(Guid token)
    {
        var resultado = await _empresaService.ValidarConviteAsync(token);

        return resultado.IsSuccess
            ? Ok(ResultadoDto<ConviteResponseDto>.Ok(resultado.Value))
            : BadRequest(ResultadoDto<object>.Falha("CONVITE_INVALIDO", "Convite inválido ou expirado."));
    }
}
