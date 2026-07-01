using FluentResults;
using TalentBridge.Application.DTOs.Dashboard;

namespace TalentBridge.Application.Interfaces;

/// <summary>
/// Serviço de dashboards e métricas
/// </summary>
public interface IDashboardService
{
    /// <summary>
    /// Obtém dashboard do candidato
    /// </summary>
    Task<Result<DashboardCandidatoResponseDto>> ObterDashboardCandidatoAsync(Guid candidatoId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém dashboard da empresa
    /// </summary>
    Task<Result<DashboardEmpresaResponseDto>> ObterDashboardEmpresaAsync(Guid empresaId, int periodoDias = 30, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém dashboard administrativo
    /// </summary>
    Task<Result<DashboardAdminResponseDto>> ObterDashboardAdminAsync(CancellationToken cancellationToken = default);
}
