namespace TalentBridge.Domain.Entities;

/// <summary>
/// Operação dentro de uma funcionalidade (ex: CANDIDATO_ATUALIZAR)
/// </summary>
public class Operacao : BaseEntity
{
    public Guid FuncionalidadeId { get; private set; }
    public string Codigo { get; private set; } = string.Empty;
    public string Descricao { get; private set; } = string.Empty;

    // Relacionamento
    public Funcionalidade Funcionalidade { get; private set; } = null!;

    protected Operacao() { }

    public Operacao(string codigo, string descricao, Guid funcionalidadeId)
    {
        Codigo = codigo.ToUpperInvariant();
        Descricao = descricao;
        FuncionalidadeId = funcionalidadeId;
    }
}
