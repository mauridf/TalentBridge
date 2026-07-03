using FluentResults;
using Microsoft.Extensions.Logging;
using TalentBridge.Application.DTOs.Dashboard;
using TalentBridge.Application.Interfaces;
using TalentBridge.Domain.Enums;
using TalentBridge.Domain.Interfaces;

namespace TalentBridge.Application.Services;

/// <summary>
/// Implementação do serviço de dashboards
/// </summary>
public class DashboardService : IDashboardService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DashboardService> _logger;

    public DashboardService(IUnitOfWork unitOfWork, ILogger<DashboardService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Obtém dashboard do candidato
    /// </summary>
    public async Task<Result<DashboardCandidatoResponseDto>> ObterDashboardCandidatoAsync(
        Guid candidatoId,
        CancellationToken cancellationToken = default)
    {
        var candidato = await _unitOfWork.Candidatos.GetByIdCompletoAsync(candidatoId, cancellationToken);
        if (candidato == null)
            return Result.Fail<DashboardCandidatoResponseDto>("CANDIDATO_NAO_ENCONTRADO");

        // Buscar candidaturas
        var candidaturas = await _unitOfWork.Candidaturas
            .GetByCandidatoIdWithVagaAsync(candidatoId, cancellationToken);
        var candidaturasList = candidaturas.ToList();

        // Buscar vagas recomendadas
        var vagasRecomendadas = await _unitOfWork.Vagas
            .GetVagasRecomendadasAsync(candidatoId, cancellationToken);

        // Calcular percentual do perfil
        var percentualPerfil = CalcularPercentualPerfil(candidato);

        // Vagas em andamento (candidatadas, não contratadas)
        var vagasEmAndamento = candidaturasList
            .Where(c => !c.Contratado)
            .Select(c => new VagaEmAndamentoDto
            {
                VagaId = c.VagaId,
                Titulo = c.Vaga?.Titulo ?? "",
                Empresa = c.Vaga?.Empresa?.Nome ?? "",
                Status = c.Contratado ? "Contratado" : c.EntrevistaRealizada ? "Entrevista realizada" : "Em análise",
                DataCandidatura = c.CreatedAt,
                EntrevistaAgendada = c.DataHoraEntrevista.HasValue,
                DataEntrevista = c.DataHoraEntrevista
            }).ToList();

        // Últimas candidaturas (top 5)
        var ultimasCandidaturas = candidaturasList
            .OrderByDescending(c => c.CreatedAt)
            .Take(5)
            .Select(c => new UltimaCandidaturaDto
            {
                CandidaturaId = c.Id,
                VagaId = c.VagaId,
                VagaTitulo = c.Vaga?.Titulo ?? "",
                Empresa = c.Vaga?.Empresa?.Nome ?? "",
                DataCandidatura = c.CreatedAt,
                Status = c.Contratado ? "Contratado" : "Em andamento"
            }).ToList();

        var dashboard = new DashboardCandidatoResponseDto
        {
            Nome = candidato.Nome,
            StatusPerfil = percentualPerfil >= 80 ? "Completo" : "Incompleto",
            PercentualPerfil = percentualPerfil,
            RealizouBigFive = candidato.RealizouTesteBigFive(),
            TotalVagasAplicadas = candidaturasList.Count,
            TotalEntrevistas = candidaturasList.Count(c => c.EntrevistaRealizada),
            TotalContratacoes = candidaturasList.Count(c => c.Contratado),
            PercentualEntrevistas = candidaturasList.Count > 0
                ? Math.Round((double)candidaturasList.Count(c => c.EntrevistaRealizada) / candidaturasList.Count * 100, 1)
                : 0,
            VagasEmAndamento = vagasEmAndamento,
            VagasRecomendadas = vagasRecomendadas.Take(5).Select(v => new VagaRecomendadaDto
            {
                VagaId = v.Id,
                Titulo = v.Titulo,
                Empresa = v.Empresa?.Nome ?? "",
                Cidade = v.Cidade,
                Estado = v.Estado,
                Salario = v.Salario,
                Compatibilidade = CalcularCompatibilidade(candidato, v)
            }).ToList(),
            UltimasCandidaturas = ultimasCandidaturas
        };

        return Result.Ok(dashboard);
    }

    /// <summary>
    /// Obtém dashboard da empresa
    /// </summary>
    public async Task<Result<DashboardEmpresaResponseDto>> ObterDashboardEmpresaAsync(
        Guid empresaId,
        int periodoDias = 30,
        CancellationToken cancellationToken = default)
    {
        var empresa = await _unitOfWork.Empresas.GetByIdAsync(empresaId, cancellationToken);
        if (empresa == null)
            return Result.Fail<DashboardEmpresaResponseDto>("EMPRESA_NAO_ENCONTRADA");

        var dataInicio = DateTime.UtcNow.AddDays(-periodoDias);

        // Buscar vagas da empresa
        var vagas = await _unitOfWork.Vagas.GetByEmpresaIdAsync(empresaId, cancellationToken);
        var vagasList = vagas.ToList();

        // Buscar candidaturas de todas as vagas da empresa
        var todasCandidaturas = new List<Domain.Entities.Candidatura>();
        foreach (var vaga in vagasList)
        {
            var candidaturasVaga = await _unitOfWork.Candidaturas
                .GetByVagaIdWithCandidatoAsync(vaga.Id, cancellationToken);
            todasCandidaturas.AddRange(candidaturasVaga);
        }

        var candidaturasPeriodo = todasCandidaturas
            .Where(c => c.CreatedAt >= dataInicio)
            .ToList();

        // Candidaturas por dia (gráfico)
        var candidaturasPorDia = candidaturasPeriodo
            .GroupBy(c => c.CreatedAt.Date)
            .Select(g => new CandidaturasPorDiaDto
            {
                Data = g.Key,
                Quantidade = g.Count()
            })
            .OrderBy(d => d.Data)
            .ToList();

        // Vagas próximas do vencimento
        var vagasProximasVencer = vagasList
            .Where(v => v.Status == StatusVagaEnum.Aberta && !v.Encerrada)
            .Select(v => new VagaProximaVencerDto
            {
                VagaId = v.Id,
                Titulo = v.Titulo,
                DataVencimento = v.DataCandidaturaFim,
                DiasRestantes = (v.DataCandidaturaFim - DateTime.UtcNow).Days,
                TotalCandidaturas = todasCandidaturas.Count(c => c.VagaId == v.Id),
                Status = v.Status.ToString()
            })
            .Where(v => v.DiasRestantes <= 7 && v.DiasRestantes >= 0)
            .OrderBy(v => v.DiasRestantes)
            .ToList();

        // Créditos
        var creditos = await _unitOfWork.CreditosEmpresas
            .FindAsync(c => c.EmpresaId == empresaId, cancellationToken: cancellationToken);
        var totalCreditos = creditos.Sum(c => c.Creditos);

        var dashboard = new DashboardEmpresaResponseDto
        {
            TotalVagasAtivas = vagasList.Count(v => v.Status == StatusVagaEnum.Aberta && !v.Encerrada),
            TotalVagasEncerradas = vagasList.Count(v => v.Encerrada),
            TotalCandidaturas = todasCandidaturas.Count,
            TotalCandidaturasPeriodo = candidaturasPeriodo.Count,
            TotalContratados = todasCandidaturas.Count(c => c.Contratado),
            MediaCandidatosPorVaga = vagasList.Count > 0
                ? Math.Round((double)todasCandidaturas.Count / vagasList.Count, 1)
                : 0,
            CreditosDisponiveis = totalCreditos,
            CandidaturasPorDia = candidaturasPorDia,
            VagasProximasVencer = vagasProximasVencer,
            UltimasCandidaturas = todasCandidaturas
                .OrderByDescending(c => c.CreatedAt)
                .Take(10)
                .Select(c => new UltimaCandidaturaEmpresaDto
                {
                    CandidaturaId = c.Id,
                    VagaId = c.VagaId,
                    VagaTitulo = c.Vaga?.Titulo ?? "",
                    CandidatoNome = c.Candidato?.Nome ?? "",
                    CandidatoEmail = c.Candidato?.Email ?? "",
                    DataCandidatura = c.CreatedAt,
                    Protocolo = c.Protocolo
                }).ToList()
        };

        return Result.Ok(dashboard);
    }

    /// <summary>
    /// Obtém dashboard administrativo
    /// </summary>
    public async Task<Result<DashboardAdminResponseDto>> ObterDashboardAdminAsync(
        CancellationToken cancellationToken = default)
    {
        var candidatos = await _unitOfWork.Candidatos.GetAllAsync(cancellationToken: cancellationToken);
        var candidatosList = candidatos.ToList();

        var empresas = await _unitOfWork.Empresas.GetAllAsync(cancellationToken: cancellationToken);
        var empresasList = empresas.ToList();

        var convites = await _unitOfWork.Convites.GetAllAsync(cancellationToken: cancellationToken);
        var convitesList = convites.ToList();

        var pedidos = await _unitOfWork.Pedidos.GetAllAsync(cancellationToken: cancellationToken);
        var pedidosList = pedidos.ToList();

        var inicioMes = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        var dashboard = new DashboardAdminResponseDto
        {
            TotalConvitesEnviados = convitesList.Count,
            ConvitesAceitos = convitesList.Count(c => c.Status == StatusConviteEnum.Aceito),
            ConvitesPendentes = convitesList.Count(c => c.Status == StatusConviteEnum.Pendente),
            TaxaConversaoConvites = convitesList.Count > 0
                ? Math.Round((double)convitesList.Count(c => c.Status == StatusConviteEnum.Aceito) / convitesList.Count * 100, 1)
                : 0,
            TotalCandidatos = candidatosList.Count,
            TotalCandidatosConfirmados = candidatosList.Count(c => c.Status == StatusUsuarioEnum.Ativo),
            TotalEmpresas = empresasList.Count,
            TotalPedidos = pedidosList.Count,
            FaturamentoMes = pedidosList
                .Where(p => p.Status == StatusPedidoEnum.PagamentoConfirmado && p.CreatedAt >= inicioMes)
                .Sum(p => p.ItensPedido.Sum(i => i.Quantidade * i.ValorUnitario)),
            StatusApi = "Online",
            StatusBanco = "Online",
            UltimaAtualizacao = DateTime.UtcNow
        };

        return Result.Ok(dashboard);
    }

    // ==========================================
    // Métodos privados
    // ==========================================

    /// <summary>
    /// Calcula o percentual de preenchimento do perfil do candidato
    /// </summary>
    private static int CalcularPercentualPerfil(Domain.Entities.Candidato candidato)
    {
        var itensPreenchidos = 0;
        var totalItens = 10;

        if (!string.IsNullOrWhiteSpace(candidato.Nome)) itensPreenchidos++;
        if (!string.IsNullOrWhiteSpace(candidato.Email)) itensPreenchidos++;
        if (!string.IsNullOrWhiteSpace(candidato.Telefone)) itensPreenchidos++;
        if (candidato.DataNascimento != default) itensPreenchidos++;
        if (candidato.PerfilPessoal != null) itensPreenchidos++;
        if (candidato.PerfilProfissional != null) itensPreenchidos++;
        if (candidato.RealizouTesteBigFive()) itensPreenchidos++;
        if (candidato.PerfilProfissional?.FormacoesAcademicas?.Any() == true) itensPreenchidos++;
        if (candidato.PerfilProfissional?.ExperienciasProfissionais?.Any() == true) itensPreenchidos++;
        if (candidato.PerfilProfissional?.CompetenciasCandidatos?.Any() == true) itensPreenchidos++;

        return (int)((double)itensPreenchidos / totalItens * 100);
    }

    /// <summary>
    /// Calcula compatibilidade entre candidato e vaga (Big Five)
    /// </summary>
    private static int CalcularCompatibilidade(Domain.Entities.Candidato candidato, Domain.Entities.Vaga vaga)
    {
        if (!candidato.RealizouTesteBigFive())
            return 50; // Sem dados, retorna 50%

        var pontuacao = 0;
        var fatores = 0;

        if (vaga.ExtroversaoMinima.HasValue && candidato.Extroversao.HasValue)
        {
            pontuacao += candidato.Extroversao.Value >= vaga.ExtroversaoMinima.Value ? 20 : 0;
            fatores++;
        }

        if (vaga.AmabilidadeMinima.HasValue && candidato.Amabilidade.HasValue)
        {
            pontuacao += candidato.Amabilidade.Value >= vaga.AmabilidadeMinima.Value ? 20 : 0;
            fatores++;
        }

        if (vaga.AutodisciplinaMinima.HasValue && candidato.Consciencia.HasValue)
        {
            pontuacao += candidato.Consciencia.Value >= vaga.AutodisciplinaMinima.Value ? 20 : 0;
            fatores++;
        }

        if (vaga.EstabilidadeEmocionalMinima.HasValue && candidato.EstabilidadeEmocional.HasValue)
        {
            pontuacao += candidato.EstabilidadeEmocional.Value >= vaga.EstabilidadeEmocionalMinima.Value ? 20 : 0;
            fatores++;
        }

        if (vaga.AberturaExperienciaMinima.HasValue && candidato.AberturaExperiencia.HasValue)
        {
            pontuacao += candidato.AberturaExperiencia.Value >= vaga.AberturaExperienciaMinima.Value ? 20 : 0;
            fatores++;
        }

        return fatores > 0 ? pontuacao / fatores : 50;
    }
}
