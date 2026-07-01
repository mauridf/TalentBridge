namespace TalentBridge.Application.DTOs.Candidatura;

/// <summary>
/// Requisição para criar uma candidatura
/// </summary>
public class CriarCandidaturaRequestDto
{
    /// <summary>
    /// ID da vaga
    /// </summary>
    public Guid VagaId { get; set; }

    /// <summary>
    /// Origem da candidatura (1=LandingPage, 2=Outros)
    /// </summary>
    public int? Origem { get; set; }
}

/// <summary>
/// Requisição para agendar entrevista
/// </summary>
public class AgendarEntrevistaRequestDto
{
    /// <summary>
    /// ID da candidatura
    /// </summary>
    public Guid CandidaturaId { get; set; }

    /// <summary>
    /// Data e hora da entrevista
    /// </summary>
    public DateTime DataHora { get; set; }

    /// <summary>
    /// Meio da entrevista (ex: "Google Meet", "Presencial")
    /// </summary>
    public string Meio { get; set; } = string.Empty;

    /// <summary>
    /// Link da videochamada (opcional)
    /// </summary>
    public string? Link { get; set; }

    /// <summary>
    /// Duração prevista em minutos
    /// </summary>
    public int? DuracaoMinutos { get; set; }
}

/// <summary>
/// Requisição para marcar como contratado
/// </summary>
public class ContratarRequestDto
{
    /// <summary>
    /// ID da candidatura
    /// </summary>
    public Guid CandidaturaId { get; set; }
}

/// <summary>
/// Resposta com dados da candidatura
/// </summary>
public class CandidaturaResponseDto
{
    public Guid Id { get; set; }
    public Guid VagaId { get; set; }
    public string VagaTitulo { get; set; } = string.Empty;
    public string EmpresaNome { get; set; } = string.Empty;
    public Guid CandidatoId { get; set; }
    public string CandidatoNome { get; set; } = string.Empty;
    public string CandidatoEmail { get; set; } = string.Empty;
    public string? Protocolo { get; set; }
    public bool Contratado { get; set; }
    public bool EntrevistaRealizada { get; set; }
    public DateTime? DataHoraEntrevista { get; set; }
    public string? MeioEntrevista { get; set; }
    public string? LinkEntrevista { get; set; }
    public int? DuracaoEntrevistaMinutos { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Filtros para busca de candidaturas
/// </summary>
public class BuscarCandidaturasRequestDto
{
    /// <summary>
    /// Filtrar por vaga
    /// </summary>
    public Guid? VagaId { get; set; }

    /// <summary>
    /// Filtrar por candidato
    /// </summary>
    public Guid? CandidatoId { get; set; }

    /// <summary>
    /// Filtrar apenas contratados
    /// </summary>
    public bool? ApenasContratados { get; set; }

    /// <summary>
    /// Filtrar por status da entrevista
    /// </summary>
    public bool? EntrevistaRealizada { get; set; }

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
