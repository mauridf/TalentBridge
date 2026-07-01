using TalentBridge.Domain.Enums;

namespace TalentBridge.Domain.Entities;

/// <summary>
/// Associação entre Treinamento e Competência
/// </summary>
public class CompetenciaTreinamento : BaseEntity
{
    public Guid TreinamentoId { get; private set; }
    public Treinamento Treinamento { get; private set; } = null!;

    public Guid CompetenciaId { get; private set; }
    public Competencia Competencia { get; private set; } = null!;

    public CompetenciaNivelEnum Nivel { get; private set; }

    protected CompetenciaTreinamento() { }

    public CompetenciaTreinamento(Guid treinamentoId, Guid competenciaId, CompetenciaNivelEnum nivel)
    {
        TreinamentoId = treinamentoId;
        CompetenciaId = competenciaId;
        Nivel = nivel;
    }
}
