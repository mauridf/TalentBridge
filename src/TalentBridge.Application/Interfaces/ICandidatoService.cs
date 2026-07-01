using FluentResults;
using TalentBridge.Application.DTOs.Candidato;

namespace TalentBridge.Application.Interfaces;

/// <summary>
/// Serviço de gestão de candidatos
/// </summary>
public interface ICandidatoService
{
    /// <summary>
    /// Cria um novo candidato
    /// </summary>
    Task<Result<CriarCandidatoResponseDto>> CriarAsync(CriarCandidatoRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Confirma o email do candidato
    /// </summary>
    Task<Result> ConfirmarEmailAsync(ConfirmarEmailRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Reenvia o email de confirmação
    /// </summary>
    Task<Result> ReenviarConfirmacaoEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca candidato por ID
    /// </summary>
    Task<Result<CandidatoResponseDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca candidato por email
    /// </summary>
    Task<Result<CandidatoResponseDto>> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Edita dados do candidato
    /// </summary>
    Task<Result<CandidatoResponseDto>> EditarAsync(Guid id, EditarCandidatoRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cria ou atualiza o perfil pessoal do candidato
    /// </summary>
    Task<Result<PerfilPessoalResponseDto>> UpsertPerfilPessoalAsync(Guid candidatoId, PerfilPessoalUpsertRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cria ou atualiza o perfil profissional do candidato
    /// </summary>
    Task<Result<PerfilProfissionalResponseDto>> UpsertPerfilProfissionalAsync(Guid candidatoId, PerfilProfissionalUpsertRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Salva o resultado do teste Big Five
    /// </summary>
    Task<Result<BigFiveResponseDto>> SalvarBigFiveAsync(Guid candidatoId, BigFiveRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém o resultado do teste Big Five
    /// </summary>
    Task<Result<BigFiveResponseDto>> GetBigFiveAsync(Guid candidatoId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém o perfil pessoal do candidato
    /// </summary>
    Task<Result<PerfilPessoalResponseDto>> GetPerfilPessoalAsync(Guid candidatoId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém o perfil profissional do candidato
    /// </summary>
    Task<Result<PerfilProfissionalResponseDto>> GetPerfilProfissionalAsync(Guid candidatoId, CancellationToken cancellationToken = default);
}
