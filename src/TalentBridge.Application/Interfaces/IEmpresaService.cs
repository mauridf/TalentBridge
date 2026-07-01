using FluentResults;
using TalentBridge.Application.DTOs.Empresa;
using TalentBridge.Application.DTOs.Usuario;

namespace TalentBridge.Application.Interfaces;

/// <summary>
/// Serviço de gestão de empresas
/// </summary>
public interface IEmpresaService
{
    /// <summary>
    /// Cria uma empresa com gestor (via convite)
    /// </summary>
    Task<Result<CriarEmpresaResponseDto>> CriarEmpresaComGestorAsync(CriarEmpresaRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cria um recrutador (via convite)
    /// </summary>
    Task<Result<ConviteResponseDto>> CriarRecrutadorAsync(Guid tokenConvite, string nome, string email, string senha, string? nomeSocial = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cria um convite para empresa ou recrutador
    /// </summary>
    Task<Result<ConviteResponseDto>> CriarConviteAsync(Guid empresaId, CriarConviteRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Valida um token de convite
    /// </summary>
    Task<Result<ConviteResponseDto>> ValidarConviteAsync(Guid token, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lista convites de uma empresa
    /// </summary>
    Task<Result<IEnumerable<ConviteResponseDto>>> ListarConvitesAsync(Guid empresaId, CancellationToken cancellationToken = default);
}
