namespace TalentBridge.Domain.Entities;

/// <summary>
/// Saldo de créditos de uma empresa por produto.
/// Tabela com unique index (EmpresaId, ProdutoId).
/// </summary>
public class CreditosEmpresa : BaseEntity
{
    public Guid EmpresaId { get; private set; }
    public Empresa Empresa { get; private set; } = null!;

    public Guid ProdutoId { get; private set; }
    public Produto Produto { get; private set; } = null!;

    /// <summary>
    /// Quantidade de créditos disponíveis
    /// </summary>
    public int Creditos { get; private set; }

    // Relacionamentos
    public ICollection<CreditoVagas> CreditoVagas { get; private set; } = new List<CreditoVagas>();
    public ICollection<HistoricoTransacao> HistoricoTransacoes { get; private set; } = new List<HistoricoTransacao>();

    protected CreditosEmpresa() { }

    public CreditosEmpresa(Guid empresaId, Guid produtoId, int creditos = 0)
    {
        EmpresaId = empresaId;
        ProdutoId = produtoId;
        Creditos = Math.Max(creditos, 0);
    }

    /// <summary>
    /// Adiciona créditos ao saldo
    /// </summary>
    public void AdicionarCreditos(int quantidade)
    {
        if (quantidade <= 0) throw new ArgumentException("Quantidade deve ser positiva", nameof(quantidade));
        Creditos += quantidade;
        AtualizarDataModificacao();
    }

    /// <summary>
    /// Consome créditos do saldo
    /// </summary>
    public bool ConsumirCreditos(int quantidade)
    {
        if (quantidade <= 0) throw new ArgumentException("Quantidade deve ser positiva", nameof(quantidade));
        if (Creditos < quantidade) return false;

        Creditos -= quantidade;
        AtualizarDataModificacao();
        return true;
    }
}
