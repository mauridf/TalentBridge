using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.DTOs.Dashboard;
using TalentBridge.Application.Interfaces;

namespace TalentBridge.Api.Controllers;

/// <summary>
/// Controller para dashboards (candidato, empresa, admin)
/// </summary>
[ApiController]
[Route("[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(IDashboardService dashboardService, ILogger<DashboardController> logger)
    {
        _dashboardService = dashboardService;
        _logger = logger;
    }

    /// <summary>
    /// Dashboard do candidato logado
    /// </summary>
    [HttpPost("candidato")]
    public async Task<IActionResult> DashboardCandidato()
    {
        var usuarioIdClaim = User.FindFirst("id")?.Value;
        if (string.IsNullOrWhiteSpace(usuarioIdClaim))
            return Unauthorized();

        var resultado = await _dashboardService.ObterDashboardCandidatoAsync(Guid.Parse(usuarioIdClaim));

        return resultado.IsSuccess
            ? Ok(ResultadoDto<DashboardCandidatoResponseDto>.Ok(resultado.Value))
            : NotFound(ResultadoDto<object>.Falha("CANDIDATO_NAO_ENCONTRADO", "Candidato não encontrado"));
    }

    /// <summary>
    /// Dashboard da empresa (gestor/recrutador)
    /// </summary>
    [HttpPost("empresa")]
    public async Task<IActionResult> DashboardEmpresa([FromBody] DashboardEmpresaRequestDto? request)
    {
        var empresaIdClaim = User.FindFirst("idEmpresa")?.Value;
        if (string.IsNullOrWhiteSpace(empresaIdClaim))
            return Unauthorized(new { erro = "Usuário não associado a uma empresa" });

        var periodo = request?.PeriodoDias ?? 30;
        var resultado = await _dashboardService.ObterDashboardEmpresaAsync(
            Guid.Parse(empresaIdClaim), periodo);

        return resultado.IsSuccess
            ? Ok(ResultadoDto<DashboardEmpresaResponseDto>.Ok(resultado.Value))
            : NotFound(ResultadoDto<object>.Falha("EMPRESA_NAO_ENCONTRADA", "Empresa não encontrada"));
    }

    /// <summary>
    /// Dashboard administrativo (apenas admin)
    /// </summary>
    [HttpPost("admin")]
    [Authorize]
    public async Task<IActionResult> DashboardAdmin()
    {
        var perfilClaim = User.FindFirst("perfil")?.Value;
        if (perfilClaim != "ADMIN")
            return Forbid();

        var resultado = await _dashboardService.ObterDashboardAdminAsync();

        return Ok(ResultadoDto<DashboardAdminResponseDto>.Ok(resultado.Value));
    }
}
