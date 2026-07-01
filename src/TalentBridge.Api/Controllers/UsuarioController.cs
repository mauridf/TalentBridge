using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.DTOs.Usuario;
using TalentBridge.Application.Interfaces;

namespace TalentBridge.Api.Controllers;

/// <summary>
/// Controller responsável pela autenticação e gestão de usuários
/// </summary>
[ApiController]
[Route("[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<UsuarioController> _logger;

    public UsuarioController(IAuthService authService, ILogger<UsuarioController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Autentica um usuário com email e senha
    /// </summary>
    [HttpPost("Autenticar")]
    [AllowAnonymous]
    public async Task<IActionResult> Autenticar([FromBody] LoginRequestDto request)
    {
        _logger.LogInformation("POST /Usuario/Autenticar - Email: {Email}", request.Email);

        var resultado = await _authService.AutenticarAsync(request);

        if (resultado.IsSuccess)
        {
            return Ok(ResultadoDto<LoginResponseDto>.Ok(resultado.Value));
        }

        // Tratar caso multi-empresa
        if (resultado.Errors.Any(e => e.Message == "MULTI_EMPRESA"))
        {
            var error = resultado.Errors.First(e => e.Message == "MULTI_EMPRESA");
            var tokenTemporario = error.Metadata["TokenTemporario"]?.ToString();
            var empresasJson = error.Metadata["Empresas"]?.ToString();

            return Ok(new
            {
                multiEmpresa = true,
                tokenTemporario,
                empresas = empresasJson
            });
        }

        var erro = resultado.Errors.First();
        return Unauthorized(ResultadoDto<object>.Falha(erro.Message, ObterMensagemErro(erro.Message)));
    }

    /// <summary>
    /// Seleciona um perfil após login multi-perfil
    /// </summary>
    [HttpPost("SelecionarPerfil")]
    [AllowAnonymous]
    public async Task<IActionResult> SelecionarPerfil([FromBody] SelecionarPerfilRequestDto request)
    {
        _logger.LogInformation("POST /Usuario/SelecionarPerfil - Perfil: {Perfil}", request.PerfilCodigo);

        var resultado = await _authService.SelecionarPerfilAsync(request);

        if (resultado.IsSuccess)
            return Ok(ResultadoDto<LoginResponseDto>.Ok(resultado.Value));

        return BadRequest(ResultadoDto<object>.Falha("PERFIL_INVALIDO", "Perfil selecionado não é válido."));
    }

    /// <summary>
    /// Seleciona uma empresa para acesso
    /// </summary>
    [HttpPost("SelecionarEmpresa")]
    [Authorize]
    public async Task<IActionResult> SelecionarEmpresa([FromBody] SelecionarEmpresaRequestDto request)
    {
        var usuarioIdClaim = User.FindFirst("id")?.Value;
        if (string.IsNullOrWhiteSpace(usuarioIdClaim))
            return Unauthorized();

        var usuarioId = Guid.Parse(usuarioIdClaim);
        var resultado = await _authService.SelecionarEmpresaAsync(usuarioId, request);

        if (resultado.IsSuccess)
            return Ok(ResultadoDto<LoginResponseDto>.Ok(resultado.Value));

        return BadRequest(ResultadoDto<object>.Falha("EMPRESA_INVALIDA", "Empresa selecionada não é válida."));
    }

    /// <summary>
    /// Renova o access token usando refresh token
    /// </summary>
    [HttpGet("RefreshAcesso")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshAcesso()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrWhiteSpace(refreshToken))
            return Unauthorized(ResultadoDto<object>.Falha("REFRESH_TOKEN_AUSENTE", "Refresh token não encontrado."));

        var resultado = await _authService.RefreshTokenAsync(refreshToken);

        if (resultado.IsSuccess)
            return Ok(ResultadoDto<RefreshTokenResponseDto>.Ok(resultado.Value));

        return Unauthorized(ResultadoDto<object>.Falha("REFRESH_TOKEN_INVALIDO", "Refresh token inválido ou expirado."));
    }

    /// <summary>
    /// Verifica se um email está disponível para cadastro
    /// </summary>
    [HttpGet("EmailDisponivel")]
    [AllowAnonymous]
    public async Task<IActionResult> EmailDisponivel([FromQuery] string email)
    {
        var resultado = await _authService.EmailDisponivelAsync(email);

        if (resultado.IsSuccess)
            return Ok(ResultadoDto<bool>.Ok(resultado.Value));

        return BadRequest(ResultadoDto<object>.Falha("ERRO_CONSULTA", "Erro ao verificar email."));
    }

    /// <summary>
    /// Envia email de recuperação de senha
    /// </summary>
    [HttpPost("ResetSenhaEmail")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetSenhaEmail([FromBody] ResetSenhaEmailRequestDto request)
    {
        var resultado = await _authService.EnviarEmailRecuperacaoSenhaAsync(request.Email);

        // Sempre retorna sucesso para não revelar se o email existe
        return Ok(new { mensagem = "Se o email existir, um link de recuperação será enviado." });
    }

    /// <summary>
    /// Redefine a senha usando token de recuperação
    /// </summary>
    [HttpPut("RecuperacaoSenha")]
    [AllowAnonymous]
    public async Task<IActionResult> RecuperacaoSenha([FromBody] RecuperacaoSenhaRequestDto request)
    {
        var resultado = await _authService.RecuperarSenhaAsync(request);

        if (resultado.IsSuccess)
            return Ok(new { mensagem = "Senha redefinida com sucesso." });

        return BadRequest(ResultadoDto<object>.Falha("TOKEN_INVALIDO", "Token de recuperação inválido ou expirado."));
    }

    /// <summary>
    /// Obtém mensagem amigável para o código de erro
    /// </summary>
    private static string ObterMensagemErro(string codigo) => codigo switch
    {
        "USUARIO_SENHA_INCORRETOS" => "Email ou senha incorretos.",
        "USUARIO_INATIVO" => "Usuário inativo. Verifique seu email para ativar a conta.",
        _ => "Erro na autenticação."
    };
}
