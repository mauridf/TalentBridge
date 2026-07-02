using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.Interfaces;

namespace TalentBridge.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class SegmentoController : ControllerBase
{
    private readonly IDominioService _dominioService;

    public SegmentoController(IDominioService dominioService)
    {
        _dominioService = dominioService;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var resultado = await _dominioService.BuscarTodosAsync();
        return resultado.IsSuccess
            ? Ok(resultado.Value)
            : BadRequest("Erro ao buscar segmentos.");
    }
}
