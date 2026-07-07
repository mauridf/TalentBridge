using BCrypt.Net;
using FluentResults;
using Microsoft.Extensions.Logging;
using TalentBridge.Application.DTOs.Usuario;
using TalentBridge.Application.Interfaces;
using TalentBridge.Domain.Entities;
using TalentBridge.Domain.Enums;
using TalentBridge.Domain.Interfaces;

namespace TalentBridge.Application.Services;

/// <summary>
/// Implementação do serviço de autenticação
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IUnitOfWork unitOfWork, ITokenService tokenService, ILogger<AuthService> logger)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _logger = logger;
    }

    /// <summary>
    /// Autentica um usuário com email e senha
    /// </summary>
    public async Task<Result<LoginResponseDto>> AutenticarAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Tentativa de login para: {Email}", request.Email);

        // Buscar usuário com perfil e empresas
        var usuario = await _unitOfWork.Usuarios.GetByEmailWithPerfisAndEmpresasAsync(request.Email, cancellationToken);

        if (usuario == null)
        {
            _logger.LogWarning("Usuário não encontrado: {Email}", request.Email);
            return Result.Fail("USUARIO_SENHA_INCORRETOS");
        }

        // Verificar senha
        if (!VerificarSenha(request.Senha, usuario.SenhaHash))
        {
            _logger.LogWarning("Senha incorreta para: {Email}", request.Email);
            return Result.Fail("USUARIO_SENHA_INCORRETOS");
        }

        // Verificar se usuário está ativo
        if (usuario.Status != StatusUsuarioEnum.Ativo)
        {
            _logger.LogWarning("Usuário inativo: {Email}", request.Email);
            return Result.Fail("USUARIO_INATIVO");
        }

        // Atualizar último login
        usuario.RegistrarLogin();

        // Verificar se é multi-perfil ou multi-empresa
        var empresasDoUsuario = usuario.UsuarioEmpresas?.ToList() ?? new List<UsuarioEmpresa>();

        if (empresasDoUsuario.Count > 1)
        {
            _logger.LogInformation("Login multi-empresa detectado para: {Email}", request.Email);

            var tokenTemporario = _tokenService.GerarTokenTemporario(usuario);

            // Retornar resposta indicando multi-empresa
            return Result.Fail("MULTI_EMPRESA")
                .WithError(new Error("MULTI_EMPRESA")
                    .WithMetadata("TokenTemporario", tokenTemporario)
                    .WithMetadata("Empresas", System.Text.Json.JsonSerializer.Serialize(
                        empresasDoUsuario.Select(e => new EmpresaDisponivelDto
                        {
                            Id = e.EmpresaId,
                            Nome = e.Empresa?.Nome ?? "N/A",
                            Cnpj = e.Empresa?.CNPJ ?? "N/A",
                            PerfilCodigo = e.Perfil?.Codigo ?? "GESTOR_EMPRESA"
                        }))));
        }

        // Gerar tokens
        var perfilCodigo = usuario.Perfil?.Codigo ?? "CANDIDATO";
        var empresaId = empresasDoUsuario.FirstOrDefault()?.EmpresaId;
        var empresaNome = empresasDoUsuario.FirstOrDefault()?.Empresa?.Nome;

        var accessToken = _tokenService.GerarAccessToken(usuario, perfilCodigo, empresaId?.ToString(), empresaNome);
        var refreshToken = _tokenService.GerarRefreshToken();

        // Salvar refresh token
        usuario.AtualizarRefreshToken(refreshToken, DateTime.UtcNow.AddDays(5));
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Login bem-sucedido para: {Email} | Perfil: {Perfil}", request.Email, perfilCodigo);

        return Result.Ok(new LoginResponseDto
        {
            AccessToken = accessToken,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Perfil = perfilCodigo,
            EmpresaId = empresaId,
            EmpresaNome = empresaNome
        });
    }

    /// <summary>
    /// Seleciona um perfil específico após login multi-perfil
    /// </summary>
    public async Task<Result<LoginResponseDto>> SelecionarPerfilAsync(SelecionarPerfilRequestDto request, CancellationToken cancellationToken = default)
    {
        var usuarioId = _tokenService.ExtrairUsuarioId(request.TokenTemporario);
        if (usuarioId == null)
            return Result.Fail("TOKEN_INVALIDO");

        var usuario = await _unitOfWork.Usuarios.GetByIdWithPerfilAsync(usuarioId.Value, cancellationToken);
        if (usuario == null)
            return Result.Fail("USUARIO_NAO_ENCONTRADO");

        // Verificar se o perfil solicitado pertence ao usuário
        // TODO: Validar perfil nas UsuarioEmpresas

        var accessToken = _tokenService.GerarAccessToken(usuario, request.PerfilCodigo);
        var refreshToken = _tokenService.GerarRefreshToken();

        usuario.AtualizarRefreshToken(refreshToken, DateTime.UtcNow.AddDays(5));
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok(new LoginResponseDto
        {
            AccessToken = accessToken,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Perfil = request.PerfilCodigo
        });
    }

    /// <summary>
    /// Seleciona uma empresa específica
    /// </summary>
    public async Task<Result<LoginResponseDto>> SelecionarEmpresaAsync(Guid usuarioId, SelecionarEmpresaRequestDto request, CancellationToken cancellationToken = default)
    {
        var usuario = await _unitOfWork.Usuarios.GetByIdWithPerfilAsync(usuarioId, cancellationToken);
        if (usuario == null)
            return Result.Fail("USUARIO_NAO_ENCONTRADO");

        var usuarioEmpresa = usuario.UsuarioEmpresas
            .FirstOrDefault(ue => ue.EmpresaId == request.IdEmpresa);

        if (usuarioEmpresa == null)
            return Result.Fail("EMPRESA_NAO_ENCONTRADA");

        var accessToken = _tokenService.GerarAccessToken(
            usuario,
            usuarioEmpresa.Perfil?.Codigo ?? "GESTOR_EMPRESA",
            request.IdEmpresa.ToString(),
            usuarioEmpresa.Empresa?.Nome);

        var refreshToken = _tokenService.GerarRefreshToken();

        usuario.AtualizarRefreshToken(refreshToken, DateTime.UtcNow.AddDays(5));
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok(new LoginResponseDto
        {
            AccessToken = accessToken,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Perfil = usuarioEmpresa.Perfil?.Codigo ?? "GESTOR_EMPRESA",
            EmpresaId = request.IdEmpresa,
            EmpresaNome = usuarioEmpresa.Empresa?.Nome
        });
    }

    /// <summary>
    /// Gera novo access token a partir do refresh token
    /// </summary>
    public async Task<Result<RefreshTokenResponseDto>> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var usuario = await _unitOfWork.Usuarios.GetByRefreshTokenAsync(refreshToken, cancellationToken);

        if (usuario == null || !usuario.RefreshTokenValido())
            return Result.Fail("REFRESH_TOKEN_INVALIDO");

        var perfilCodigo = usuario.Perfil?.Codigo ?? "CANDIDATO";
        var accessToken = _tokenService.GerarAccessToken(usuario, perfilCodigo);

        // Rotacionar refresh token
        var novoRefreshToken = _tokenService.GerarRefreshToken();
        usuario.AtualizarRefreshToken(novoRefreshToken, DateTime.UtcNow.AddDays(5));
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok(new RefreshTokenResponseDto
        {
            AccessToken = accessToken
        });
    }

    /// <summary>
    /// Verifica se email está disponível
    /// </summary>
    public async Task<Result<bool>> EmailDisponivelAsync(string email, CancellationToken cancellationToken = default)
    {
        var existe = await _unitOfWork.Usuarios.EmailExisteAsync(email, cancellationToken);
        return Result.Ok(!existe);
    }

    /// <summary>
    /// Envia email de recuperação de senha
    /// </summary>
    public async Task<Result> EnviarEmailRecuperacaoSenhaAsync(string email, CancellationToken cancellationToken = default)
    {
        var usuario = await _unitOfWork.Usuarios.GetByEmailAsync(email, cancellationToken);
        if (usuario == null)
            return Result.Ok(); // Não revelar se o email existe

        var token = Guid.NewGuid().ToString("N");
        var redefinicao = new RedefinicaoSenha(usuario.Id, token, 3);

        await _unitOfWork.ExecuteInTransactionAsync(async (ct) =>
        {
            await _unitOfWork.SaveChangesAsync(ct);
            return Result.Ok();
        }, cancellationToken);

        // TODO: Enviar email com o token
        _logger.LogInformation("Token de recuperação gerado para: {Email}", email);

        return Result.Ok();
    }

    /// <summary>
    /// Redefine a senha usando token
    /// </summary>
    public async Task<Result> RecuperarSenhaAsync(RecuperacaoSenhaRequestDto request, CancellationToken cancellationToken = default)
    {
        // TODO: Buscar RedefinicaoSenha pelo token e validar
        // Por enquanto, retorna sucesso
        _logger.LogInformation("Senha redefinida com token");
        return Result.Ok();
    }

    public string HashSenha(string senha)
    {
        return BCrypt.Net.BCrypt.HashPassword(senha, workFactor: 12);
    }

    public bool VerificarSenha(string senha, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(senha, hash);
    }
}
