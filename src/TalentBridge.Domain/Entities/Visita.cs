namespace TalentBridge.Domain.Entities;

/// <summary>
/// Registro de visualização de uma vaga por um candidato.
/// Usado para analytics e tracking.
/// </summary>
public class Visita : BaseEntity
{
    public Guid VagaId { get; private set; }
    public Vaga Vaga { get; private set; } = null!;

    public Guid CandidatoId { get; private set; }
    public Candidato Candidato { get; private set; } = null!;

    /// <summary>
    /// Data e hora da visualização
    /// </summary>
    public DateTime DataVisita { get; private set; }

    /// <summary>
    /// IP do visitante
    /// </summary>
    public string? Ip { get; private set; }

    /// <summary>
    /// User Agent do navegador
    /// </summary>
    public string? UserAgent { get; private set; }

    /// <summary>
    /// Origem da visita (ex: "Google", "LinkedIn", "Direto")
    /// </summary>
    public string? Origem { get; private set; }

    protected Visita() { }

    public Visita(Guid vagaId, Guid candidatoId, string? ip = null, string? userAgent = null, string? origem = null)
    {
        VagaId = vagaId;
        CandidatoId = candidatoId;
        DataVisita = DateTime.UtcNow;
        Ip = ip;
        UserAgent = userAgent;
        Origem = origem;
    }
}
