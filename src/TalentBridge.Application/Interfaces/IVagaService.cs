using FluentResults;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.DTOs.Vaga;

namespace TalentBridge.Application.Interfaces;

/// <summary>
/// Serviço de gestão de vagas
/// </summary>
public interface IVagaService
{
    /// <summary>
    /// Cria ou edita uma vaga
    /// </summary>
    Task<Result<VagaResponseDto>> UpsertAsync(Guid empresaId, Guid usuarioId, VagaUpsertRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca vagas ativas com filtros e paginação
    /// </summary>
    Task<Result<PaginacaoResponseDto<VagaResponseDto>>> BuscarVagasAsync(BuscarVagasRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca vaga por ID
    /// </summary>
    Task<Result<VagaResponseDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lista vagas de uma empresa
    /// </summary>
    Task<Result<IEnumerable<VagaResponseDto>>> GetByEmpresaAsync(Guid empresaId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Encerra uma vaga
    /// </summary>
    Task<Result> EncerrarAsync(Guid vagaId, Guid usuarioId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Reativa uma vaga encerrada
    /// </summary>
    Task<Result> ReativarAsync(Guid vagaId, CancellationToken cancellationToken = default);
}
