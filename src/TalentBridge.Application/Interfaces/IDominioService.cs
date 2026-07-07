using FluentResults;
using TalentBridge.Domain.Enums;

namespace TalentBridge.Application.Interfaces;

/// <summary>
/// Serviço para consulta de domínios (lookup table)
/// </summary>
public interface IDominioService
{
    /// <summary>
    /// Busca domínios por tipo
    /// </summary>
    Task<Result<IEnumerable<DominioDto>>> BuscarPorTipoAsync(TipoDominioEnum tipo, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca todos os domínios agrupados por tipo
    /// </summary>
    Task<Result<Dictionary<string, List<DominioDto>>>> BuscarTodosAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// DTO de domínio
/// </summary>
public class DominioDto
{
    public Guid Id { get; set; }
    public int Codigo { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int Tipo { get; set; }
    public bool Ativo { get; set; }
}
