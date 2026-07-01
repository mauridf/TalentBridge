using FluentResults;
using TalentBridge.Application.DTOs.LandingPage;

namespace TalentBridge.Application.Interfaces;

/// <summary>
/// Serviço para landing pages públicas
/// </summary>
public interface ILandingPageService
{
    /// <summary>
    /// Obtém dados da landing page de uma empresa
    /// </summary>
    Task<Result<LandingPageResponseDto>> ObterLandingPageAsync(Guid empresaId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém detalhe de uma vaga para landing page
    /// </summary>
    Task<Result<VagaDetalheLandingPageDto>> ObterDetalheVagaAsync(Guid vagaId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca empresa por slug
    /// </summary>
    Task<Result<LandingPageResponseDto>> ObterPorSlugAsync(string slug, CancellationToken cancellationToken = default);
}
