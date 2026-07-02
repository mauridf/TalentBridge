namespace TalentBridge.Domain.Entities;

public class ItemIncluido : BaseEntity
{
    public Guid TreinamentoId { get; private set; }
    public Treinamento Treinamento { get; private set; } = null!;

    public string Descricao { get; private set; } = string.Empty;

    protected ItemIncluido() { }

    public ItemIncluido(Guid treinamentoId, string descricao)
    {
        TreinamentoId = treinamentoId;
        Descricao = descricao;
    }
}
