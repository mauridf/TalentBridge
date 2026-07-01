using FluentResults;
using TalentBridge.Application.DTOs.Candidatura;
using TalentBridge.Application.DTOs.Common;

namespace TalentBridge.Application.Interfaces;

/// <summary>
/// Serviço de gestão de candidaturas
/// </summary>
public interface ICandidaturaService
{
    /// <summary>
    /// Cria uma nova candidatura
    /// </summary>
    Task<Result<CandidaturaResponseDto>> CriarAsync(Guid candidatoId, CriarCandidaturaRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica se já existe candidatura para a vaga
    /// </summary>
    Task<Result<bool>> VerificarCandidaturaExistenteAsync(Guid vagaId, Guid candidatoId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca candidaturas com filtros
    /// </summary>
    Task<Result<IEnumerable<CandidaturaResponseDto>>> BuscarAsync(BuscarCandidaturasRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca candidaturas de uma vaga (para o gestor/recrutador)
    /// </summary>
    Task<Result<IEnumerable<CandidaturaResponseDto>>> GetByVagaAsync(Guid vagaId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca candidaturas de um candidato
    /// </summary>
    Task<Result<IEnumerable<CandidaturaResponseDto>>> GetByCandidatoAsync(Guid candidatoId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Agenda uma entrevista
    /// </summary>
    Task<Result> AgendarEntrevistaAsync(AgendarEntrevistaRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Marca entrevista como realizada
    /// </summary>
    Task<Result> MarcarEntrevistaRealizadaAsync(Guid candidaturaId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Marca candidato como contratado
    /// </summary>
    Task<Result> ContratarAsync(Guid candidaturaId, CancellationToken cancellationToken = default);
}
