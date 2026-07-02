using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.DTOs.Usuario;
using TalentBridge.Application.Interfaces;

namespace TalentBridge.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConviteController : ControllerBase
{
    private readonly IConviteService _conviteService;
    private readonly ILogger<ConviteController> _logger;

    public ConviteController(IConviteService conviteService, ILogger<ConviteController> logger)
    {
        _conviteService = conviteService;
        _logger = logger;
    }

    [HttpPost]
    [Authorize(Roles = "ADMIN,GESTOR_EMPRESA")]
    public async Task<IActionResult> Criar([FromBody] CriarConviteRequestDto request)
    {
        _logger.LogInformation("POST /api/Convite - Criar convite");
        var empresaIdClaim = User.FindFirst("idEmpresa")?.Value;
        if (string.IsNullOrWhiteSpace(empresaIdClaim))
            return Unauthorized(ResultadoDto<object>.Falha("SEM_EMPRESA", "Usuário não está associado a uma empresa."));

        var resultado = await _conviteService.CriarConvite(request, Guid.Parse(empresaIdClaim));
        return resultado.Sucesso
            ? Created(string.Empty, ResultadoDto<ConviteResponseDto>.Ok(resultado.Valor))
            : BadRequest(ResultadoDto<object>.Falha("ERRO_CONVITE", resultado.Erros?.FirstOrDefault()?.Mensagem ?? "Erro ao criar convite."));
    }

    [HttpPost("Validar")]
    [AllowAnonymous]
    public async Task<IActionResult> Validar([FromBody] ValidarConviteRequestDto request)
    {
        _logger.LogInformation("POST /api/Convite/Validar");
        var resultado = await _conviteService.ValidarToken(request.Token.ToString());
        return resultado.Sucesso
            ? Ok(ResultadoDto<ConviteResponseDto>.Ok(resultado.Valor))
            : BadRequest(ResultadoDto<object>.Falha("CONVITE_INVALIDO", "Convite inválido ou expirado."));
    }

    [HttpGet("empresa/{empresaId}")]
    [Authorize(Roles = "GESTOR_EMPRESA")]
    public async Task<IActionResult> ListarPorEmpresa(Guid empresaId)
    {
        var resultado = await _conviteService.ListarPorEmpresa(empresaId);
        return Ok(ResultadoDto<List<ConviteResponseDto>>.Ok(resultado.Valor));
    }

    [HttpPost("{id}/inativar")]
    [Authorize(Roles = "GESTOR_EMPRESA")]
    public async Task<IActionResult> Inativar(Guid id)
    {
        _logger.LogInformation("POST /api/Convite/{Id}/inativar", id);
        var resultado = await _conviteService.Inativar(id);
        return resultado.Sucesso
            ? Ok(new { mensagem = "Convite inativado com sucesso." })
            : BadRequest(ResultadoDto<object>.Falha("ERRO_INATIVAR", "Erro ao inativar convite."));
    }
}
