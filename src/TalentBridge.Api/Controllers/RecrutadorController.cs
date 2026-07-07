using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.DTOs.Usuario;
using TalentBridge.Application.Interfaces;

namespace TalentBridge.Api.Controllers;

/// <summary>
/// Controller responsável pela gestão de recrutadores
/// </summary>
[ApiController]
[Route("[controller]")]
public class RecrutadorController : ControllerBase
{
    private readonly IRecrutadorService _recrutadorService;
    private readonly ILogger<RecrutadorController> _logger;

    public RecrutadorController(
        IRecrutadorService recrutadorService,
        ILogger<RecrutadorController> logger)
    {
        _recrutadorService = recrutadorService;
        _logger = logger;
    }

    /// <summary>
    /// Cria um recrutador via convite (público)
    /// </summary>
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Criar([FromBody] CriarRecrutadorRequestDto request)
    {
        _logger.LogInformation("POST /Recrutador - Email: {Email}", request.Email);

        var resultado = await _recrutadorService.CriarAsync(request);

        if (resultado.Sucesso)
            return Created(string.Empty, resultado);

        return BadRequest(resultado);
    }

    /// <summary>
    /// Cria um recrutador diretamente pelo gestor da empresa (autenticado)
    /// </summary>
    [HttpPost("direto")]
    [Authorize]
    public async Task<IActionResult> CriarDireto([FromBody] CriarRecrutadorDiretoRequestDto request)
    {
        var empresaIdClaim = User.FindFirst("idEmpresa")?.Value;
        if (string.IsNullOrEmpty(empresaIdClaim) || !Guid.TryParse(empresaIdClaim, out var empresaId))
        {
            return Unauthorized();
        }

        _logger.LogInformation("POST /Recrutador/direto - Email: {Email} | Empresa: {EmpresaId}",
            request.Email, empresaId);

        var resultado = await _recrutadorService.CriarDiretoAsync(request, empresaId);

        if (resultado.Sucesso)
            return Created(string.Empty, resultado);

        return BadRequest(resultado);
    }

    /// <summary>
    /// Lista recrutadores da empresa do gestor logado
    /// </summary>
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Listar()
    {
        var empresaIdClaim = User.FindFirst("idEmpresa")?.Value;
        if (string.IsNullOrEmpty(empresaIdClaim) || !Guid.TryParse(empresaIdClaim, out var empresaId))
        {
            return Unauthorized();
        }

        var resultado = await _recrutadorService.ListarPorEmpresaAsync(empresaId);

        return Ok(resultado);
    }
}
