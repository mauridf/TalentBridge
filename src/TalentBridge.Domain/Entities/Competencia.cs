namespace TalentBridge.Domain.Entities;

/// <summary>
/// Competência (skill) que pode ser associada a vagas e candidatos.
/// Entidade compartilhada entre empresa, vaga e candidato.
/// </summary>
public class Competencia : BaseEntity
{
    /// <summary>
    /// Nome da competência (ex: "C#", "Inglês", "Liderança")
    /// </summary>
    public string Nome { get; private set; } = string.Empty;

    /// <summary>
    /// Empresa que criou a competência (null para competências globais)
    /// </summary>
    public Guid? EmpresaId { get; private set; }
    public Empresa? Empresa { get; private set; }

    // Relacionamentos
    public ICollection<CompetenciaVaga> CompetenciasVagas { get; private set; } = new List<CompetenciaVaga>();
    public ICollection<CompetenciaCandidato> CompetenciasCandidatos { get; private set; } = new List<CompetenciaCandidato>();
    public ICollection<CompetenciaTreinamento> CompetenciasTreinamentos { get; private set; } = new List<CompetenciaTreinamento>();

    protected Competencia() { }

    public Competencia(string nome, Guid? empresaId = null)
    {
        Nome = nome.Trim();
        EmpresaId = empresaId;
    }
}
