using TalentBridge.Domain.Entities;

namespace TalentBridge.Domain.Interfaces.Repositories;

/// <summary>
/// Repositório específico para a entidade Vaga
/// </summary>
public interface IVagaRepository : IRepository<Vaga>
{
    /// <summary>
    /// Busca vagas ativas com filtros
    /// </summary>
    Task<IEnumerable<Vaga>> BuscarVagasAtivasAsync(
        string? termo = null,
        string? cidade = null,
        string? estado = null,
        int? areaAtuacao = null,
        int? regimeTrabalho = null,
        int? tipoContratacao = null,
        int pagina = 1,
        int tamanhoPagina = 10,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca vagas de uma empresa
    /// </summary>
    Task<IEnumerable<Vaga>> GetByEmpresaIdAsync(Guid empresaId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca vagas recomendadas para um candidato (baseado em perfil)
    /// </summary>
    Task<IEnumerable<Vaga>> GetVagasRecomendadasAsync(Guid candidatoId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca vagas que estão próximas do vencimento
    /// </summary>
    Task<IEnumerable<Vaga>> GetProximasVencerAsync(Guid empresaId, int diasLimite = 7, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca vagas similares a uma vaga específica
    /// </summary>
    Task<IEnumerable<Vaga>> GetSimilaresAsync(Guid vagaId, int limite = 5, CancellationToken cancellationToken = default);
}
