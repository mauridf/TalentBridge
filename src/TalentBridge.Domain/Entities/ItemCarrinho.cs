namespace TalentBridge.Domain.Entities;

/// <summary>
/// Item do carrinho de compras
/// </summary>
public class ItemCarrinho : BaseEntity
{
    public Guid CarrinhoId { get; private set; }
    public Carrinho Carrinho { get; private set; } = null!;

    public Guid ProdutoId { get; private set; }
    public Produto Produto { get; private set; } = null!;

    /// <summary>
    /// Quantidade do produto
    /// </summary>
    public int Quantidade { get; private set; }

    /// <summary>
    /// Valor unitário no momento da adição ao carrinho
    /// </summary>
    public decimal ValorUnitario { get; private set; }

    protected ItemCarrinho() { }

    public ItemCarrinho(Guid carrinhoId, Guid produtoId, int quantidade, decimal valorUnitario)
    {
        CarrinhoId = carrinhoId;
        ProdutoId = produtoId;
        Quantidade = Math.Max(quantidade, 1);
        ValorUnitario = Math.Max(valorUnitario, 0);
    }

    /// <summary>
    /// Atualiza a quantidade do item
    /// </summary>
    public void AtualizarQuantidade(int quantidade)
    {
        Quantidade = Math.Max(quantidade, 1);
        AtualizarDataModificacao();
    }
}
