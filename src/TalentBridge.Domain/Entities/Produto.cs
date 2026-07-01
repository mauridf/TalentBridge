namespace TalentBridge.Domain.Entities;

/// <summary>
/// Produto disponível para compra (créditos para vagas)
/// </summary>
public class Produto : BaseEntity
{
    /// <summary>
    /// Nome do produto
    /// </summary>
    public string NomeProduto { get; private set; } = string.Empty;

    /// <summary>
    /// Descrição do produto
    /// </summary>
    public string? DescricaoProduto { get; private set; }

    /// <summary>
    /// Valor do produto em reais
    /// </summary>
    public decimal ValorProduto { get; private set; }

    /// <summary>
    /// Quantidade de candidatos (para produtos de banco de currículos)
    /// </summary>
    public int? QuantidadeCandidatos { get; private set; }

    /// <summary>
    /// Quantidade de créditos por vaga que este produto concede
    /// </summary>
    public int QuantidadeCreditoPorVaga { get; private set; } = 1;

    // Relacionamentos
    public ICollection<CreditosEmpresa> CreditosEmpresas { get; private set; } = new List<CreditosEmpresa>();
    public ICollection<ItemCarrinho> ItensCarrinho { get; private set; } = new List<ItemCarrinho>();
    public ICollection<ItemPedido> ItensPedido { get; private set; } = new List<ItemPedido>();

    protected Produto() { }

    public Produto(string nomeProduto, decimal valorProduto, int quantidadeCreditoPorVaga = 1, string? descricaoProduto = null, int? quantidadeCandidatos = null)
    {
        NomeProduto = nomeProduto;
        ValorProduto = Math.Max(valorProduto, 0);
        QuantidadeCreditoPorVaga = Math.Max(quantidadeCreditoPorVaga, 1);
        DescricaoProduto = descricaoProduto;
        QuantidadeCandidatos = quantidadeCandidatos;
    }
}
