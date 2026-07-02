using TalentBridge.Application.DTOs.Candidato;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.DTOs.Dashboard;
using TalentBridge.Application.DTOs.Empresa;

namespace TalentBridge.Application.Interfaces;

public class MonitorResponseDto
{
    public string ApiStatus { get; set; } = string.Empty;
    public string DbStatus { get; set; } = string.Empty;
    public Dictionary<string, string>? ExternalApis { get; set; }
}

public interface IAdminService
{
    Task<ResultadoDto<DashboardAdminResponseDto>> ObterDashboard();
    Task<ResultadoDto<List<EmpresaResponseDto>>> ListarEmpresas();
    Task<ResultadoDto<List<CandidatoResponseDto>>> ListarCandidatos();
    Task<ResultadoDto<bool>> IniciarVarreduraGeocode();
    Task<ResultadoDto<MonitorResponseDto>> ObterMonitor();
}
