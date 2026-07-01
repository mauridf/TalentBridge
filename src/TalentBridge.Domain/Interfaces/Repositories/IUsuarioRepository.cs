using TalentBridge.Domain.Entities;

namespace TalentBridge.Domain.Interfaces.Repositories;

/// <summary>
/// Repositório específico para a entidade Usuario
/// </summary>
public interface IUsuarioRepository : IRepository<Usuario>
{
    /// <summary>
    /// Busca usuário por email
    /// </summary>
    Task<Usuario?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca usuário com perfil incluso
    /// </summary>
    Task<Usuario?> GetByIdWithPerfilAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca usuário por email com refresh token incluso
    /// </summary>
    Task<Usuario?> GetByEmailWithRefreshTokenAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca usuário por refresh token
    /// </summary>
    Task<Usuario?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica se email já existe
    /// </summary>
    Task<bool> EmailExisteAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca usuário por email com perfis e empresas
    /// </summary>
    Task<Usuario?> GetByEmailWithPerfisAndEmpresasAsync(string email, CancellationToken cancellationToken = default);
}
