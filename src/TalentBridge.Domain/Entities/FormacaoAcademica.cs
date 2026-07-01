namespace TalentBridge.Domain.Entities;

/// <summary>
/// Formação acadêmica de um candidato
/// </summary>
public class FormacaoAcademica : BaseEntity
{
    public Guid PerfilProfissionalId { get; private set; }
    public PerfilProfissional PerfilProfissional { get; private set; } = null!;

    /// <summary>
    /// Grau da formação (ex: "Bacharelado", "Mestrado")
    /// </summary>
    public string Grau { get; private set; } = string.Empty;

    /// <summary>
    /// Área de atuação/curso
    /// </summary>
    public string AreaAtuacao { get; private set; } = string.Empty;

    /// <summary>
    /// Data de conclusão
    /// </summary>
    public DateTime? DataConclusao { get; private set; }

    /// <summary>
    /// Indica se a formação foi concluída
    /// </summary>
    public bool Concluido { get; private set; }

    protected FormacaoAcademica() { }

    public FormacaoAcademica(Guid perfilProfissionalId, string grau, string areaAtuacao, DateTime? dataConclusao = null, bool concluido = false)
    {
        PerfilProfissionalId = perfilProfissionalId;
        Grau = grau;
        AreaAtuacao = areaAtuacao;
        DataConclusao = dataConclusao;
        Concluido = concluido;
    }
}
