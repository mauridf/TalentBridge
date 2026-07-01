using TalentBridge.Domain.Enums;

namespace TalentBridge.Domain.Entities;

/// <summary>
/// Cupom de desconto para compra de créditos
/// </summary>
public class Cupom : BaseEntity
{
    /// <summary>
    /// Nome/código do cupom
    /// </summary>
    public string Nome { get; private set; } = string.Empty;

    /// <summary>
    /// Percentual de desconto (0-100)
    /// </summary>
    public int PercentualDesconto { get; private set; }

    /// <summary>
    /// Data de validade do cupom
    /// </summary>
    public DateTime DataValidade { get; private set; }

    /// <summary>
    /// Parceiro associado ao cupom (se aplicável)
    /// </summary>
    public Guid? ParceiroId { get; private set; }
    public Parceiro? Parceiro { get; private set; }

    /// <summary>
    /// Status atual do cupom
    /// </summary>
    public StatusCupomEnum Status { get; private set; } = StatusCupomEnum.Ativo;

    // Relacionamentos
    public ICollection<Pedido> Pedidos { get; private set; } = new List<Pedido>();

    protected Cupom() { }

    public Cupom(string nome, int percentualDesconto, DateTime dataValidade, Guid? parceiroId = null)
    {
        Nome = nome.ToUpperInvariant().Trim();
        PercentualDesconto = Math.Clamp(percentualDesconto, 0, 100);
        DataValidade = dataValidade;
        ParceiroId = parceiroId;
    }

    /// <summary>
    /// Verifica se o cupom está válido para uso
    /// </summary>
    public bool EstaValido()
    {
        return Status == StatusCupomEnum.Ativo && DateTime.UtcNow <= DataValidade;
    }
}
