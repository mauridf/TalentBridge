using FluentResults;
using TalentBridge.Application.DTOs.Usuario;

namespace TalentBridge.Application.Interfaces;

/// <summary>
/// Serviço de autenticação e autorização
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Autentica um usuário com email e senha
    /// </summary>
    Task<Result<LoginResponseDto>> AutenticarAsync(LoginRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Seleciona um perfil específico após login multi-perfil
    /// </summary>
    Task<Result<LoginResponseDto>> SelecionarPerfilAsync(SelecionarPerfilRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Seleciona uma empresa específica após login multi-empresa
    /// </summary>
    Task<Result<LoginResponseDto>> SelecionarEmpresaAsync(Guid usuarioId, SelecionarEmpresaRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gera novo access token a partir do refresh token
    /// </summary>
    Task<Result<RefreshTokenResponseDto>> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica se um email está disponível para cadastro
    /// </summary>
    Task<Result<bool>> EmailDisponivelAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Inicia o processo de recuperação de senha
    /// </summary>
    Task<Result> EnviarEmailRecuperacaoSenhaAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Redefine a senha usando token de recuperação
    /// </summary>
    Task<Result> RecuperarSenhaAsync(RecuperacaoSenhaRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gera o hash de uma senha usando BCrypt
    /// </summary>
    string HashSenha(string senha);

    /// <summary>
    /// Verifica se uma senha corresponde ao hash
    /// </summary>
    bool VerificarSenha(string senha, string hash);
}
