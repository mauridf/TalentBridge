using TalentBridge.Domain.Entities;

namespace TalentBridge.Domain.Interfaces.Repositories;

/// <summary>
/// Repositório específico para a entidade Candidatura
/// </summary>
public interface ICandidaturaRepository : IRepository<Candidatura>
{
    /// <summary>
    /// Verifica se já existe candidatura para a vaga e candidato
    /// </summary>
    Task<bool> ExisteCandidaturaAsync(Guid vagaId, Guid candidatoId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca candidaturas de uma vaga com dados do candidato
    /// </summary>
    Task<IEnumerable<Candidatura>> GetByVagaIdWithCandidatoAsync(Guid vagaId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca candidaturas de um candidato com dados da vaga
    /// </summary>
    Task<IEnumerable<Candidatura>> GetByCandidatoIdWithVagaAsync(Guid candidatoId, CancellationToken cancellationToken = default);
}
