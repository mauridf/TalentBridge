using Microsoft.Extensions.Logging;
using TalentBridge.Application.DTOs.Candidato;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.DTOs.Dashboard;
using TalentBridge.Application.DTOs.Empresa;
using TalentBridge.Application.Interfaces;
using TalentBridge.Domain.Enums;
using TalentBridge.Domain.Interfaces;

namespace TalentBridge.Application.Services;

public class AdminService : IAdminService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AdminService> _logger;

    public AdminService(IUnitOfWork unitOfWork, ILogger<AdminService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ResultadoDto<DashboardAdminResponseDto>> ObterDashboard()
    {
        _logger.LogInformation("Obtendo dashboard administrativo.");

        var convites = await _unitOfWork.Convites.GetAllAsync();
        var convitesList = convites.ToList();

        var candidatos = await _unitOfWork.Candidatos.GetAllAsync();
        var candidatosList = candidatos.ToList();

        var empresas = await _unitOfWork.Empresas.GetAllAsync();
        var empresasList = empresas.ToList();

        var vagas = await _unitOfWork.Vagas.GetAllAsync();
        var vagasList = vagas.ToList();

        var pedidos = await _unitOfWork.Pedidos.GetAllAsync();
        var pedidosList = pedidos.ToList();

        var dashboard = new DashboardAdminResponseDto
        {
            TotalConvitesEnviados = convitesList.Count,
            ConvitesAceitos = convitesList.Count(c => c.Status == StatusConviteEnum.Aceito),
            ConvitesPendentes = convitesList.Count(c => c.Status == StatusConviteEnum.Pendente),
            TaxaConversaoConvites = convitesList.Count > 0
                ? Math.Round((double)convitesList.Count(c => c.Status == StatusConviteEnum.Aceito) / convitesList.Count * 100, 2)
                : 0,
            TotalCandidatos = candidatosList.Count,
            TotalCandidatosConfirmados = candidatosList.Count(c => c.Status == StatusUsuarioEnum.Ativo),
            TotalEmpresas = empresasList.Count,
            TotalVagasAtivas = vagasList.Count(v => v.Status == StatusVagaEnum.Aberta && !v.Encerrada),
            TotalCandidaturas = (await _unitOfWork.Candidaturas.GetAllAsync()).Count(),
            TotalPedidos = pedidosList.Count,
            FaturamentoTotal = pedidosList.Where(p => p.Status == StatusPedidoEnum.Finalizado).Sum(p => 0m),
            FaturamentoMes = pedidosList.Where(p => p.Status == StatusPedidoEnum.Finalizado && p.CreatedAt.Month == DateTime.UtcNow.Month && p.CreatedAt.Year == DateTime.UtcNow.Year).Sum(p => 0m),
            StatusApi = "Online",
            StatusBanco = "Online",
            UltimaAtualizacao = DateTime.UtcNow
        };

        return ResultadoDto<DashboardAdminResponseDto>.Ok(dashboard);
    }

    public async Task<ResultadoDto<List<EmpresaResponseDto>>> ListarEmpresas()
    {
        _logger.LogInformation("Listando empresas.");

        var empresas = await _unitOfWork.Empresas.GetAllAsync();

        var result = empresas.Select(e => new EmpresaResponseDto
        {
            Id = e.Id,
            Nome = e.Nome,
            Cnpj = e.CNPJ,
            Email = e.Email,
            Telefone = e.Telefone
        }).ToList();

        return ResultadoDto<List<EmpresaResponseDto>>.Ok(result);
    }

    public async Task<ResultadoDto<List<CandidatoResponseDto>>> ListarCandidatos()
    {
        _logger.LogInformation("Listando candidatos.");

        var candidatos = await _unitOfWork.Candidatos.GetAllAsync();

        var result = candidatos.Select(c => new CandidatoResponseDto
        {
            Id = c.Id,
            Nome = c.Nome,
            Email = c.Email,
            Telefone = c.Telefone,
            Status = c.Status.ToString(),
            CreatedAt = c.CreatedAt
        }).ToList();

        return ResultadoDto<List<CandidatoResponseDto>>.Ok(result);
    }

    public Task<ResultadoDto<bool>> IniciarVarreduraGeocode()
    {
        _logger.LogWarning("AdminService.IniciarVarreduraGeocode não implementado.");
        throw new NotImplementedException("AdminService.IniciarVarreduraGeocode ainda não foi implementado.");
    }

    public async Task<ResultadoDto<MonitorResponseDto>> ObterMonitor()
    {
        _logger.LogInformation("Obtendo monitor do sistema.");

        var monitor = new MonitorResponseDto
        {
            ApiStatus = "Online",
            DbStatus = "Online",
            ExternalApis = new Dictionary<string, string>
            {
                { "Asaas", "Não configurado" },
                { "ViaCEP", "Não configurado" },
                { "GoogleJobs", "Não configurado" }
            }
        };

        return ResultadoDto<MonitorResponseDto>.Ok(monitor);
    }
}
