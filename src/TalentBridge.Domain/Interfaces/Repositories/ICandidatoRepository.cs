using TalentBridge.Domain.Entities;

namespace TalentBridge.Domain.Interfaces.Repositories;

/// <summary>
/// Repositório específico para a entidade Candidato
/// </summary>
public interface ICandidatoRepository : IRepository<Candidato>
{
    /// <summary>
    /// Busca candidato por email
    /// </summary>
    Task<Candidato?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca candidato completo com perfil pessoal e profissional
    /// </summary>
    Task<Candidato?> GetByIdCompletoAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca candidato por token de confirmação de email
    /// </summary>
    Task<Candidato?> GetByTokenConfirmacaoAsync(Guid token, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lista candidatos de um parceiro
    /// </summary>
    Task<IEnumerable<Candidato>> GetByParceiroIdAsync(Guid parceiroId, CancellationToken cancellationToken = default);
}
