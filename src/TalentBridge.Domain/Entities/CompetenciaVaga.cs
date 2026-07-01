using TalentBridge.Domain.Enums;

namespace TalentBridge.Domain.Entities;

/// <summary>
/// Associação entre Vaga e Competência com nível e peso.
/// Tabela de junção N:N com dados adicionais.
/// </summary>
public class CompetenciaVaga : BaseEntity
{
    public Guid VagaId { get; private set; }
    public Vaga Vaga { get; private set; } = null!;

    public Guid CompetenciaId { get; private set; }
    public Competencia Competencia { get; private set; } = null!;

    /// <summary>
    /// Nível requerido: Basico=1, Moderado=2, Avancado=3
    /// </summary>
    public CompetenciaNivelEnum Nivel { get; private set; }

    /// <summary>
    /// Peso da competência na avaliação (1-10)
    /// </summary>
    public int Peso { get; private set; }

    protected CompetenciaVaga() { }

    public CompetenciaVaga(Guid vagaId, Guid competenciaId, CompetenciaNivelEnum nivel, int peso)
    {
        VagaId = vagaId;
        CompetenciaId = competenciaId;
        Nivel = nivel;
        Peso = Math.Clamp(peso, 1, 10);
    }
}
