namespace TalentBridge.Application.DTOs.Dashboard;

/// <summary>
/// Requisição para o dashboard do candidato
/// </summary>
public class DashboardCandidatoRequestDto
{
    /// <summary>
    /// ID do candidato (opcional, usa o logado se não informado)
    /// </summary>
    public Guid? CandidatoId { get; set; }
}

/// <summary>
/// Resposta do dashboard do candidato
/// </summary>
public class DashboardCandidatoResponseDto
{
    /// <summary>
    /// Nome do candidato
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Status do perfil (completo/incompleto)
    /// </summary>
    public string StatusPerfil { get; set; } = string.Empty;

    /// <summary>
    /// Percentual de preenchimento do perfil
    /// </summary>
    public int PercentualPerfil { get; set; }

    /// <summary>
    /// Se já realizou o teste Big Five
    /// </summary>
    public bool RealizouBigFive { get; set; }

    // Métricas de vagas
    public int TotalVagasAplicadas { get; set; }
    public int TotalEntrevistas { get; set; }
    public int TotalContratacoes { get; set; }
    public int TotalVagasVisualizadas { get; set; }

    // Percentuais
    public double PercentualEntrevistas { get; set; }

    // Vagas em andamento
    public List<VagaEmAndamentoDto> VagasEmAndamento { get; set; } = new();

    // Vagas recomendadas (até 5)
    public List<VagaRecomendadaDto> VagasRecomendadas { get; set; } = new();

    // Últimas candidaturas
    public List<UltimaCandidaturaDto> UltimasCandidaturas { get; set; } = new();

    // Curso gratuito
    public bool CursoGratuitoVisualizado { get; set; }
    public string? CursoGratuitoLink { get; set; }
}

/// <summary>
/// Vaga em andamento (candidatou-se e está no processo)
/// </summary>
public class VagaEmAndamentoDto
{
    public Guid VagaId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Empresa { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime DataCandidatura { get; set; }
    public bool EntrevistaAgendada { get; set; }
    public DateTime? DataEntrevista { get; set; }
}

/// <summary>
/// Vaga recomendada baseada no perfil
/// </summary>
public class VagaRecomendadaDto
{
    public Guid VagaId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Empresa { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public decimal Salario { get; set; }
    public int Compatibilidade { get; set; } // 0-100
}

/// <summary>
/// Última candidatura realizada
/// </summary>
public class UltimaCandidaturaDto
{
    public Guid CandidaturaId { get; set; }
    public Guid VagaId { get; set; }
    public string VagaTitulo { get; set; } = string.Empty;
    public string Empresa { get; set; } = string.Empty;
    public DateTime DataCandidatura { get; set; }
    public string Status { get; set; } = string.Empty;
}
