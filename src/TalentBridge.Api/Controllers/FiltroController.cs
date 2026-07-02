using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.Interfaces;

namespace TalentBridge.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class FiltroController : ControllerBase
{
    private readonly IFiltroService _filtroService;

    public FiltroController(IFiltroService filtroService)
    {
        _filtroService = filtroService;
    }

    [HttpGet]
    public async Task<IActionResult> ObterFiltros()
    {
        var resultado = await _filtroService.ObterFiltros();
        return Ok(resultado.Valor);
    }
}
