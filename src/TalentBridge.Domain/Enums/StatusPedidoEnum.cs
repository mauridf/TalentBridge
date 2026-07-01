namespace TalentBridge.Domain.Enums;

/// <summary>
/// Status do pedido (baseado nos estados do Asaas)
/// </summary>
public enum StatusPedidoEnum
{
    Criado = 0,
    AguardandoPagamento = 1,
    PagamentoEmAnalise = 2,
    PagamentoConfirmado = 3,
    PagamentoRecebidoEmConta = 4,
    PagamentoRecusado = 5,
    PagamentoEstornado = 6,
    PagamentoEstornoNegado = 7,
    PagamentoVencido = 8,
    CheckoutExpirado = 9,
    CheckoutCancelado = 10,
    PagamentoFalhaCapturaCartaoCredito = 11,
    Cancelado = 12,
    Finalizado = 13
}
