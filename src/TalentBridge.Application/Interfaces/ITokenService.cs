using System.Security.Claims;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Application.Interfaces;

/// <summary>
/// Serviço para geração e validação de tokens JWT
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Gera um access token JWT
    /// </summary>
    string GerarAccessToken(Usuario usuario, string perfilCodigo, string? empresaId = null, string? empresaNome = null);

    /// <summary>
    /// Gera um refresh token
    /// </summary>
    string GerarRefreshToken();

    /// <summary>
    /// Gera um token temporário para seleção de perfil
    /// </summary>
    string GerarTokenTemporario(Usuario usuario);

    /// <summary>
    /// Valida um token e retorna as claims
    /// </summary>
    ClaimsPrincipal? ValidarToken(string token);

    /// <summary>
    /// Extrai o ID do usuário de um token
    /// </summary>
    Guid? ExtrairUsuarioId(string token);

    /// <summary>
    /// Gera um token para convite
    /// </summary>
    string GerarTokenConvite(Convite convite);
}
