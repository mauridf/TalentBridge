namespace TalentBridge.Application.DTOs.Credito;

/// <summary>
/// Resposta com dados do pedido
/// </summary>
public class PedidoResponseDto
{
    public Guid Id { get; set; }
    public int NumeroPedido { get; set; }
    public Guid EmpresaId { get; set; }
    public string EmpresaNome { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal ValorTotal { get; set; }
    public decimal? ValorDesconto { get; set; }
    public decimal ValorFinal { get; set; }
    public string? CupomCodigo { get; set; }
    public string? IdCheckout { get; set; }
    public string? LinkPagamento { get; set; }
    public bool CreditosInseridos { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<ItemPedidoResponseDto> Itens { get; set; } = new();
}

/// <summary>
/// Item do pedido na resposta
/// </summary>
public class ItemPedidoResponseDto
{
    public Guid ProdutoId { get; set; }
    public string NomeProduto { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
    public decimal ValorTotal { get; set; }
}

/// <summary>
/// Requisição para listar pedidos por CNPJ
/// </summary>
public class ListarPedidosRequestDto
{
    public string Cnpj { get; set; } = string.Empty;
}

/// <summary>
/// Dados do checkout Asaas
/// </summary>
public class CheckoutResponseDto
{
    public string IdCheckout { get; set; } = string.Empty;
    public string LinkPagamento { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

/// <summary>
/// Requisição do webhook Asaas
/// </summary>
public class AsaasWebhookRequestDto
{
    public string Event { get; set; } = string.Empty;
    public PaymentData? Payment { get; set; }
}

public class PaymentData
{
    public string Id { get; set; } = string.Empty;
    public string ExternalReference { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public decimal NetValue { get; set; }
    public string BillingType { get; set; } = string.Empty;
    public DateTime DateCreated { get; set; }
}

/// <summary>
/// Resumo de créditos da empresa
/// </summary>
public class CreditosResponseDto
{
    public Guid EmpresaId { get; set; }
    public string EmpresaNome { get; set; } = string.Empty;
    public int TotalCreditos { get; set; }
    public int CreditosUsados { get; set; }
    public int CreditosDisponiveis { get; set; }
    public List<CreditoPorProdutoDto> CreditosPorProduto { get; set; } = new();
}

public class CreditoPorProdutoDto
{
    public Guid ProdutoId { get; set; }
    public string NomeProduto { get; set; } = string.Empty;
    public int Creditos { get; set; }
}
