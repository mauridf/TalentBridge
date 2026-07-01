namespace TalentBridge.Domain.Entities;

/// <summary>
/// Item de um pedido
/// </summary>
public class ItemPedido : BaseEntity
{
    public Guid PedidoId { get; private set; }
    public Pedido Pedido { get; private set; } = null!;

    public Guid ProdutoId { get; private set; }
    public Produto Produto { get; private set; } = null!;

    /// <summary>
    /// Quantidade comprada
    /// </summary>
    public int Quantidade { get; private set; }

    /// <summary>
    /// Valor unitário no momento da compra
    /// </summary>
    public decimal ValorUnitario { get; private set; }

    protected ItemPedido() { }

    public ItemPedido(Guid pedidoId, Guid produtoId, int quantidade, decimal valorUnitario)
    {
        PedidoId = pedidoId;
        ProdutoId = produtoId;
        Quantidade = Math.Max(quantidade, 1);
        ValorUnitario = Math.Max(valorUnitario, 0);
    }
}
