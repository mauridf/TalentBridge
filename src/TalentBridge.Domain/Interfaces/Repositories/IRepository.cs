using TalentBridge.Domain.Entities;
using System.Linq.Expressions;

namespace TalentBridge.Domain.Interfaces.Repositories;

/// <summary>
/// Interface genérica para repositórios com operações CRUD básicas
/// </summary>
/// <typeparam name="T">Tipo da entidade (deve herdar de BaseEntity)</typeparam>
public interface IRepository<T> where T : BaseEntity
{
    /// <summary>
    /// Busca uma entidade pelo ID
    /// </summary>
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca todas as entidades (com opção de tracking)
    /// </summary>
    Task<IEnumerable<T>> GetAllAsync(bool asNoTracking = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca entidades por expressão de filtro
    /// </summary>
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca uma única entidade por expressão de filtro
    /// </summary>
    Task<T?> FindSingleAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica se existe alguma entidade que atenda ao filtro
    /// </summary>
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adiciona uma nova entidade
    /// </summary>
    Task AddAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adiciona múltiplas entidades
    /// </summary>
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Atualiza uma entidade existente
    /// </summary>
    void Update(T entity);

    /// <summary>
    /// Remove uma entidade (hard delete)
    /// </summary>
    void Remove(T entity);

    /// <summary>
    /// Remove múltiplas entidades
    /// </summary>
    void RemoveRange(IEnumerable<T> entities);
}
