using FluentResults;
using TalentBridge.Application.DTOs.Credito;

namespace TalentBridge.Application.Interfaces;

/// <summary>
/// Serviço de gestão de créditos e pagamentos
/// </summary>
public interface ICreditoService
{
    /// <summary>
    /// Lista todos os produtos disponíveis
    /// </summary>
    Task<Result<IEnumerable<ProdutoResponseDto>>> ListarProdutosAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém ou cria o carrinho ativo da empresa
    /// </summary>
    Task<Result<CarrinhoResponseDto>> ObterCarrinhoAsync(Guid empresaId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adiciona ou atualiza um item no carrinho
    /// </summary>
    Task<Result<CarrinhoResponseDto>> AtualizarCarrinhoAsync(Guid empresaId, AtualizarCarrinhoRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove um item do carrinho
    /// </summary>
    Task<Result<CarrinhoResponseDto>> RemoverItemCarrinhoAsync(Guid empresaId, Guid produtoId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finaliza o carrinho e cria um pedido
    /// </summary>
    Task<Result<PedidoResponseDto>> FinalizarCarrinhoAsync(Guid empresaId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lista pedidos de uma empresa pelo CNPJ
    /// </summary>
    Task<Result<IEnumerable<PedidoResponseDto>>> ListarPedidosAsync(string cnpj, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém detalhes de um pedido
    /// </summary>
    Task<Result<PedidoResponseDto>> ObterPedidoAsync(Guid pedidoId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Realiza checkout no Asaas (mock inicial)
    /// </summary>
    Task<Result<CheckoutResponseDto>> RealizarCheckoutAsync(Guid pedidoId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Processa webhook do Asaas
    /// </summary>
    Task<Result> ProcessarWebhookAsync(AsaasWebhookRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém o saldo de créditos da empresa
    /// </summary>
    Task<Result<CreditosResponseDto>> ObterCreditosAsync(Guid empresaId, CancellationToken cancellationToken = default);
}
