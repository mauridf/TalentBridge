namespace TalentBridge.Domain.Entities;

/// <summary>
/// Perfil profissional do candidato (experiências, formações, skills)
/// </summary>
public class PerfilProfissional : BaseEntity
{
    /// <summary>
    /// Indica se o candidato dispensa experiência profissional
    /// </summary>
    public bool DispensaExperienciaProfissional { get; private set; }

    // Relacionamentos
    public Candidato? Candidato { get; private set; }
    public ICollection<CompetenciaCandidato> CompetenciasCandidatos { get; private set; } = new List<CompetenciaCandidato>();
    public ICollection<FormacaoAcademica> FormacoesAcademicas { get; private set; } = new List<FormacaoAcademica>();
    public ICollection<ExperienciaProfissional> ExperienciasProfissionais { get; private set; } = new List<ExperienciaProfissional>();
    public ICollection<Curso> Cursos { get; private set; } = new List<Curso>();
    public ICollection<AreaInteresse> AreasInteresse { get; private set; } = new List<AreaInteresse>();

    protected PerfilProfissional() { }

    public PerfilProfissional(bool dispensaExperienciaProfissional = false)
    {
        DispensaExperienciaProfissional = dispensaExperienciaProfissional;
    }

    /// <summary>
    /// Adiciona uma competência ao perfil profissional
    /// </summary>
    public void AdicionarCompetencia(CompetenciaCandidato competencia)
    {
        CompetenciasCandidatos.Add(competencia);
        AtualizarDataModificacao();
    }

    /// <summary>
    /// Adiciona uma formação acadêmica
    /// </summary>
    public void AdicionarFormacaoAcademica(FormacaoAcademica formacao)
    {
        FormacoesAcademicas.Add(formacao);
        AtualizarDataModificacao();
    }

    /// <summary>
    /// Adiciona uma experiência profissional
    /// </summary>
    public void AdicionarExperienciaProfissional(ExperienciaProfissional experiencia)
    {
        ExperienciasProfissionais.Add(experiencia);
        AtualizarDataModificacao();
    }
}
