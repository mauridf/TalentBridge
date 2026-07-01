using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentBridge.Application.Interfaces;

namespace TalentBridge.Api.Controllers;

/// <summary>
/// Controller para landing pages públicas
/// </summary>
[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class LandingPageController : ControllerBase
{
    private readonly ILandingPageService _landingPageService;

    public LandingPageController(ILandingPageService landingPageService)
    {
        _landingPageService = landingPageService;
    }

    /// <summary>
    /// Obtém landing page de uma empresa com vagas abertas
    /// </summary>
    [HttpGet("{empresaId}")]
    public async Task<IActionResult> ObterLandingPage(Guid empresaId)
    {
        var resultado = await _landingPageService.ObterLandingPageAsync(empresaId);
        return resultado.IsSuccess ? Ok(resultado.Value) : NotFound();
    }

    /// <summary>
    /// Obtém detalhe de uma vaga para landing page
    /// </summary>
    [HttpGet("vaga/{vagaId}")]
    public async Task<IActionResult> ObterDetalheVaga(Guid vagaId)
    {
        var resultado = await _landingPageService.ObterDetalheVagaAsync(vagaId);
        return resultado.IsSuccess ? Ok(resultado.Value) : NotFound();
    }

    /// <summary>
    /// Busca empresa por slug
    /// </summary>
    [HttpGet("empresa/{slug}")]
    public async Task<IActionResult> ObterPorSlug(string slug)
    {
        var resultado = await _landingPageService.ObterPorSlugAsync(slug);
        return resultado.IsSuccess ? Ok(resultado.Value) : NotFound();
    }
}
