using TalentBridge.Domain.Enums;

namespace TalentBridge.Domain.Entities;

/// <summary>
/// Pedido gerado a partir da finalização de um carrinho
/// </summary>
public class Pedido : BaseEntity
{
    /// <summary>
    /// Número sequencial do pedido
    /// </summary>
    public int NumeroPedido { get; private set; }

    public Guid CarrinhoId { get; private set; }
    public Carrinho Carrinho { get; private set; } = null!;

    public Guid EmpresaId { get; private set; }
    public Empresa Empresa { get; private set; } = null!;

    public Guid? CupomId { get; private set; }
    public Cupom? Cupom { get; private set; }

    /// <summary>
    /// ID do checkout no Asaas
    /// </summary>
    public string? IdCheckout { get; private set; }

    /// <summary>
    /// Status atual do pedido
    /// </summary>
    public StatusPedidoEnum Status { get; private set; } = StatusPedidoEnum.Criado;

    /// <summary>
    /// Indica se os créditos já foram inseridos na conta da empresa
    /// </summary>
    public bool CreditosInseridos { get; private set; }

    // Relacionamentos
    public ICollection<ItemPedido> ItensPedido { get; private set; } = new List<ItemPedido>();

    protected Pedido() { }

    public Pedido(int numeroPedido, Guid carrinhoId, Guid empresaId, Guid? cupomId = null)
    {
        NumeroPedido = numeroPedido;
        CarrinhoId = carrinhoId;
        EmpresaId = empresaId;
        CupomId = cupomId;
        Status = StatusPedidoEnum.Criado;
    }

    /// <summary>
    /// Atualiza o status do pagamento
    /// </summary>
    public void AtualizarStatus(StatusPedidoEnum novoStatus)
    {
        Status = novoStatus;
        AtualizarDataModificacao();
    }

    /// <summary>
    /// Confirma o pagamento e libera os créditos
    /// </summary>
    public void ConfirmarPagamento(string idCheckout)
    {
        IdCheckout = idCheckout;
        Status = StatusPedidoEnum.PagamentoConfirmado;
        AtualizarDataModificacao();
    }

    /// <summary>
    /// Marca que os créditos foram inseridos
    /// </summary>
    public void MarcarCreditosInseridos()
    {
        CreditosInseridos = true;
        Status = StatusPedidoEnum.Finalizado;
        AtualizarDataModificacao();
    }
}
