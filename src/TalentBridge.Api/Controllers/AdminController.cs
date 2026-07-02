using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.Interfaces;

namespace TalentBridge.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "ADMIN")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;
    private readonly ILogger<AdminController> _logger;

    public AdminController(IAdminService adminService, ILogger<AdminController> logger)
    {
        _adminService = adminService;
        _logger = logger;
    }

    [HttpPost("Dashboard")]
    public async Task<IActionResult> Dashboard()
    {
        var resultado = await _adminService.ObterDashboard();
        return resultado.Sucesso
            ? Ok(resultado)
            : BadRequest(resultado);
    }

    [HttpPost("GeocodeMissing")]
    public async Task<IActionResult> IniciarGeocode()
    {
        var resultado = await _adminService.IniciarVarreduraGeocode();
        return resultado.Sucesso
            ? Ok(resultado)
            : BadRequest(resultado);
    }

    [HttpGet("Monitor")]
    public async Task<IActionResult> Monitor()
    {
        var resultado = await _adminService.ObterMonitor();
        return Ok(resultado);
    }

    [HttpGet("GeocodeMissing/Empresas")]
    public async Task<IActionResult> GeocodeMissingEmpresas()
    {
        return Ok(await _adminService.ListarEmpresas());
    }

    [HttpGet("GeocodeStatus/latest")]
    public IActionResult GeocodeStatusLatest()
    {
        return Ok(ResultadoDto<object>.Ok(new { status = "Funcionalidade em desenvolvimento" }));
    }
}
