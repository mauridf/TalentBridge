namespace TalentBridge.Domain.Entities;

/// <summary>
/// Registro de consumo de créditos para uma vaga específica
/// </summary>
public class CreditoVagas : BaseEntity
{
    public Guid EmpresaId { get; private set; }
    public Empresa Empresa { get; private set; } = null!;

    public Guid VagaId { get; private set; }
    public Vaga Vaga { get; private set; } = null!;

    public Guid ProdutoId { get; private set; }
    public Produto Produto { get; private set; } = null!;

    public Guid CreditoEmpresaId { get; private set; }
    public CreditosEmpresa CreditoEmpresa { get; private set; } = null!;

    /// <summary>
    /// Quantidade de créditos liberados para esta vaga
    /// </summary>
    public int QuantidadeLiberada { get; private set; }

    // Relacionamento
    public ICollection<HistoricoTransacao> HistoricoTransacoes { get; private set; } = new List<HistoricoTransacao>();

    protected CreditoVagas() { }

    public CreditoVagas(Guid empresaId, Guid vagaId, Guid produtoId, Guid creditoEmpresaId, int quantidadeLiberada = 1)
    {
        EmpresaId = empresaId;
        VagaId = vagaId;
        ProdutoId = produtoId;
        CreditoEmpresaId = creditoEmpresaId;
        QuantidadeLiberada = Math.Max(quantidadeLiberada, 1);
    }
}
