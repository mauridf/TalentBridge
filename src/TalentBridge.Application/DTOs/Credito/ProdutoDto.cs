namespace TalentBridge.Application.DTOs.Credito;

/// <summary>
/// Resposta com dados do produto
/// </summary>
public class ProdutoResponseDto
{
    public Guid Id { get; set; }
    public string NomeProduto { get; set; } = string.Empty;
    public string? DescricaoProduto { get; set; }
    public decimal ValorProduto { get; set; }
    public int QuantidadeCreditoPorVaga { get; set; }
    public int? QuantidadeCandidatos { get; set; }
}
