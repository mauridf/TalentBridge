namespace TalentBridge.Domain.Entities;

/// <summary>
/// Experiência profissional de um candidato
/// </summary>
public class ExperienciaProfissional : BaseEntity
{
    public Guid PerfilProfissionalId { get; private set; }
    public PerfilProfissional PerfilProfissional { get; private set; } = null!;

    /// <summary>
    /// Nome da empresa
    /// </summary>
    public string Empresa { get; private set; } = string.Empty;

    /// <summary>
    /// Cargo/posição ocupada
    /// </summary>
    public string Posicao { get; private set; } = string.Empty;

    /// <summary>
    /// Data de início
    /// </summary>
    public DateTime DataInicio { get; private set; }

    /// <summary>
    /// Data de fim (null se emprego atual)
    /// </summary>
    public DateTime? DataFim { get; private set; }

    /// <summary>
    /// Indica se é o emprego atual
    /// </summary>
    public bool EmpregoAtual { get; private set; }

    protected ExperienciaProfissional() { }

    public ExperienciaProfissional(Guid perfilProfissionalId, string empresa, string posicao, DateTime dataInicio, DateTime? dataFim = null, bool empregoAtual = false)
    {
        PerfilProfissionalId = perfilProfissionalId;
        Empresa = empresa;
        Posicao = posicao;
        DataInicio = dataInicio;
        DataFim = dataFim;
        EmpregoAtual = empregoAtual;
    }
}
