namespace TalentBridge.Application.DTOs.Dashboard;

/// <summary>
/// Requisição para o dashboard da empresa
/// </summary>
public class DashboardEmpresaRequestDto
{
    /// <summary>
    /// Período para filtrar métricas (dias, padrão 30)
    /// </summary>
    public int PeriodoDias { get; set; } = 30;
}

/// <summary>
/// Resposta do dashboard da empresa
/// </summary>
public class DashboardEmpresaResponseDto
{
    // Métricas gerais
    public int TotalVagasAtivas { get; set; }
    public int TotalVagasEncerradas { get; set; }
    public int TotalCandidaturas { get; set; }
    public int TotalCandidaturasPeriodo { get; set; }
    public int TotalVisitas { get; set; }
    public int TotalContratados { get; set; }
    public double MediaCandidatosPorVaga { get; set; }

    // Créditos
    public int CreditosDisponiveis { get; set; }
    public int CreditosUsados { get; set; }

    // Gráfico: candidaturas por dia
    public List<CandidaturasPorDiaDto> CandidaturasPorDia { get; set; } = new();

    // Melhores candidatos (compatibilidade Big Five)
    public List<MelhorCandidatoDto> MelhoresCandidatos { get; set; } = new();

    // Últimas candidaturas
    public List<UltimaCandidaturaEmpresaDto> UltimasCandidaturas { get; set; } = new();

    // Vagas próximas do vencimento
    public List<VagaProximaVencerDto> VagasProximasVencer { get; set; } = new();

    // Recrutadores ativos
    public int TotalRecrutadores { get; set; }
}

/// <summary>
/// Candidaturas agrupadas por dia
/// </summary>
public class CandidaturasPorDiaDto
{
    public DateTime Data { get; set; }
    public int Quantidade { get; set; }
}

/// <summary>
/// Melhor candidato ranqueado
/// </summary>
public class MelhorCandidatoDto
{
    public Guid CandidatoId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Cidade { get; set; }
    public int Compatibilidade { get; set; }
    public double? DistanciaKm { get; set; }
    public bool RealizouBigFive { get; set; }
}

/// <summary>
/// Última candidatura recebida
/// </summary>
public class UltimaCandidaturaEmpresaDto
{
    public Guid CandidaturaId { get; set; }
    public Guid VagaId { get; set; }
    public string VagaTitulo { get; set; } = string.Empty;
    public string CandidatoNome { get; set; } = string.Empty;
    public string CandidatoEmail { get; set; } = string.Empty;
    public DateTime DataCandidatura { get; set; }
    public string? Protocolo { get; set; }
}

/// <summary>
/// Vaga próxima do vencimento
/// </summary>
public class VagaProximaVencerDto
{
    public Guid VagaId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public DateTime DataVencimento { get; set; }
    public int DiasRestantes { get; set; }
    public int TotalCandidaturas { get; set; }
    public string Status { get; set; } = string.Empty;
}
