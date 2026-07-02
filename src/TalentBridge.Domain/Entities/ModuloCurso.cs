namespace TalentBridge.Domain.Entities;

public class ModuloCurso : BaseEntity
{
    public Guid TreinamentoId { get; private set; }
    public Treinamento Treinamento { get; private set; } = null!;

    public string Nome { get; private set; } = string.Empty;
    public string? Descricao { get; private set; }
    public int Ordem { get; private set; }
    public int DuracaoMinutos { get; private set; }

    public ICollection<ConteudoModulo> Conteudos { get; private set; } = new List<ConteudoModulo>();

    protected ModuloCurso() { }

    public ModuloCurso(Guid treinamentoId, string nome, int ordem, int duracaoMinutos, string? descricao = null)
    {
        TreinamentoId = treinamentoId;
        Nome = nome;
        Ordem = ordem;
        DuracaoMinutos = duracaoMinutos;
        Descricao = descricao;
    }
}
