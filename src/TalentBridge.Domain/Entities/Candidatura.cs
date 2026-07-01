namespace TalentBridge.Domain.Entities;

/// <summary>
/// Candidatura de um candidato a uma vaga
/// </summary>
public class Candidatura : BaseEntity
{
    public Guid VagaId { get; private set; }
    public Vaga Vaga { get; private set; } = null!;

    public Guid CandidatoId { get; private set; }
    public Candidato Candidato { get; private set; } = null!;

    /// <summary>
    /// Indica se o candidato foi contratado para esta vaga
    /// </summary>
    public bool Contratado { get; private set; }

    /// <summary>
    /// Indica se a entrevista foi realizada
    /// </summary>
    public bool EntrevistaRealizada { get; private set; }

    /// <summary>
    /// Data e hora da entrevista agendada
    /// </summary>
    public DateTime? DataHoraEntrevista { get; private set; }

    /// <summary>
    /// Link para a entrevista (videochamada)
    /// </summary>
    public string? LinkEntrevista { get; private set; }

    /// <summary>
    /// Meio da entrevista (ex: "Google Meet", "Presencial")
    /// </summary>
    public string? MeioEntrevista { get; private set; }

    /// <summary>
    /// Duração prevista da entrevista em minutos
    /// </summary>
    public int? DuracaoEntrevistaMinutos { get; private set; }

    /// <summary>
    /// Origem da candidatura (1=LandingPage, 2=Outros)
    /// </summary>
    public int? Origem { get; private set; }

    /// <summary>
    /// Protocolo único da candidatura
    /// </summary>
    public string? Protocolo { get; private set; }

    protected Candidatura() { }

    public Candidatura(Guid vagaId, Guid candidatoId, int? origem = null)
    {
        VagaId = vagaId;
        CandidatoId = candidatoId;
        Origem = origem;
        Protocolo = GerarProtocolo();
    }

    /// <summary>
    /// Agenda uma entrevista para o candidato
    /// </summary>
    public void AgendarEntrevista(DateTime dataHora, string meio, string? link = null, int? duracaoMinutos = null)
    {
        DataHoraEntrevista = dataHora;
        MeioEntrevista = meio;
        LinkEntrevista = link;
        DuracaoEntrevistaMinutos = duracaoMinutos;
        AtualizarDataModificacao();
    }

    /// <summary>
    /// Marca a entrevista como realizada
    /// </summary>
    public void MarcarEntrevistaRealizada()
    {
        EntrevistaRealizada = true;
        AtualizarDataModificacao();
    }

    /// <summary>
    /// Marca o candidato como contratado
    /// </summary>
    public void Contratar()
    {
        Contratado = true;
        AtualizarDataModificacao();
    }

    /// <summary>
    /// Gera um protocolo único para a candidatura
    /// </summary>
    private static string GerarProtocolo()
    {
        return $"TB-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpperInvariant()}";
    }
}
