using TalentBridge.Domain.Enums;

namespace TalentBridge.Domain.Entities;

/// <summary>
/// Associação entre Candidato (via PerfilProfissional) e Competência com nível.
/// Tabela de junção N:N com dados adicionais.
/// </summary>
public class CompetenciaCandidato : BaseEntity
{
    public Guid PerfilProfissionalId { get; private set; }
    public PerfilProfissional PerfilProfissional { get; private set; } = null!;

    public Guid CompetenciaId { get; private set; }
    public Competencia Competencia { get; private set; } = null!;

    /// <summary>
    /// Nível de proficiência: Basico=1, Moderado=2, Avancado=3
    /// </summary>
    public CompetenciaNivelEnum Nivel { get; private set; }

    protected CompetenciaCandidato() { }

    public CompetenciaCandidato(Guid perfilProfissionalId, Guid competenciaId, CompetenciaNivelEnum nivel)
    {
        PerfilProfissionalId = perfilProfissionalId;
        CompetenciaId = competenciaId;
        Nivel = nivel;
    }
}
