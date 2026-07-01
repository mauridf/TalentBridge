namespace TalentBridge.Application.DTOs.Credito;

/// <summary>
/// Requisição para adicionar/atualizar item no carrinho
/// </summary>
public class AtualizarCarrinhoRequestDto
{
    /// <summary>
    /// ID do produto
    /// </summary>
    public Guid ProdutoId { get; set; }

    /// <summary>
    /// Quantidade desejada
    /// </summary>
    public int Quantidade { get; set; } = 1;

    /// <summary>
    /// Código do cupom de desconto (opcional)
    /// </summary>
    public string? CodigoCupom { get; set; }
}

/// <summary>
/// Resposta com dados do carrinho
/// </summary>
public class CarrinhoResponseDto
{
    public Guid Id { get; set; }
    public Guid EmpresaId { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<ItemCarrinhoResponseDto> Itens { get; set; } = new();
    public decimal ValorTotal { get; set; }
    public decimal? ValorDesconto { get; set; }
    public decimal ValorFinal { get; set; }
    public string? CodigoCupom { get; set; }
}

/// <summary>
/// Item do carrinho na resposta
/// </summary>
public class ItemCarrinhoResponseDto
{
    public Guid Id { get; set; }
    public Guid ProdutoId { get; set; }
    public string NomeProduto { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
    public decimal ValorTotal { get; set; }
}
