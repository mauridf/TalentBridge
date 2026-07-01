using TalentBridge.Domain.Enums;

namespace TalentBridge.Domain.Entities;

/// <summary>
/// Carrinho de compras de uma empresa
/// </summary>
public class Carrinho : BaseEntity
{
    public Guid EmpresaId { get; private set; }
    public Empresa Empresa { get; private set; } = null!;

    /// <summary>
    /// Status atual do carrinho
    /// </summary>
    public StatusCarrinhoEnum Status { get; private set; } = StatusCarrinhoEnum.Ativo;

    // Relacionamentos
    public ICollection<ItemCarrinho> ItensCarrinho { get; private set; } = new List<ItemCarrinho>();

    protected Carrinho() { }

    public Carrinho(Guid empresaId)
    {
        EmpresaId = empresaId;
        Status = StatusCarrinhoEnum.Ativo;
    }

    /// <summary>
    /// Finaliza o carrinho
    /// </summary>
    public void Finalizar()
    {
        Status = StatusCarrinhoEnum.Finalizado;
        AtualizarDataModificacao();
    }

    /// <summary>
    /// Cancela o carrinho
    /// </summary>
    public void Cancelar()
    {
        Status = StatusCarrinhoEnum.Cancelado;
        AtualizarDataModificacao();
    }

    /// <summary>
    /// Calcula o valor total do carrinho
    /// </summary>
    public decimal CalcularTotal()
    {
        return ItensCarrinho.Sum(i => i.ValorUnitario * i.Quantidade);
    }
}
