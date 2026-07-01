using TalentBridge.Domain.Entities;

namespace TalentBridge.Domain.Interfaces.Repositories;

/// <summary>
/// Repositório específico para a entidade Empresa
/// </summary>
public interface IEmpresaRepository : IRepository<Empresa>
{
    /// <summary>
    /// Busca empresa por CNPJ
    /// </summary>
    Task<Empresa?> GetByCnpjAsync(string cnpj, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca empresa por ID de usuário associado
    /// </summary>
    Task<IEnumerable<Empresa>> GetByUsuarioIdAsync(Guid usuarioId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca empresas de um parceiro
    /// </summary>
    Task<IEnumerable<Empresa>> GetByParceiroIdAsync(Guid parceiroId, CancellationToken cancellationToken = default);
}
