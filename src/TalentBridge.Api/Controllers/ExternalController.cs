using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentBridge.Infrastructure.Services;

namespace TalentBridge.Api.Controllers;

/// <summary>
/// Controller para APIs externas (CEP, Estados)
/// </summary>
[ApiController]
[Route("[controller]")]
[AllowAnonymous]
public class ExternalController : ControllerBase
{
    private readonly ViaCepService _viaCepService;

    public ExternalController(ViaCepService viaCepService)
    {
        _viaCepService = viaCepService;
    }

    /// <summary>
    /// Consulta endereço por CEP
    /// </summary>
    [HttpGet("cep/{cep}")]
    public async Task<IActionResult> ConsultarCep(string cep)
    {
        var resultado = await _viaCepService.ConsultarCepAsync(cep);

        return resultado != null
            ? Ok(resultado)
            : NotFound(new { erro = "CEP não encontrado" });
    }

    /// <summary>
    /// Lista estados brasileiros
    /// </summary>
    [HttpGet("estados")]
    public IActionResult ListarEstados()
    {
        var estados = _viaCepService.ListarEstados();
        return Ok(estados);
    }
}
