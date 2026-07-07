using FluentResults;
using Microsoft.Extensions.Logging;
using TalentBridge.Application.DTOs.Credito;
using TalentBridge.Application.Interfaces;
using TalentBridge.Domain.Entities;
using TalentBridge.Domain.Enums;
using TalentBridge.Domain.Interfaces;

namespace TalentBridge.Application.Services;

/// <summary>
/// Implementação do serviço de créditos e pagamentos
/// </summary>
public class CreditoService : ICreditoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreditoService> _logger;

    public CreditoService(IUnitOfWork unitOfWork, ILogger<CreditoService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Lista todos os produtos disponíveis
    /// </summary>
    public async Task<Result<IEnumerable<ProdutoResponseDto>>> ListarProdutosAsync(
        CancellationToken cancellationToken = default)
    {
        var produtos = await _unitOfWork.Produtos.GetAllAsync(cancellationToken: cancellationToken);

        var dtos = produtos.Select(p => new ProdutoResponseDto
        {
            Id = p.Id,
            NomeProduto = p.NomeProduto,
            DescricaoProduto = p.DescricaoProduto,
            ValorProduto = p.ValorProduto,
            QuantidadeCreditoPorVaga = p.QuantidadeCreditoPorVaga,
            QuantidadeCandidatos = p.QuantidadeCandidatos
        });

        return Result.Ok(dtos);
    }

    /// <summary>
    /// Obtém ou cria o carrinho ativo da empresa
    /// </summary>
    public async Task<Result<CarrinhoResponseDto>> ObterCarrinhoAsync(
        Guid empresaId,
        CancellationToken cancellationToken = default)
    {
        var carrinho = await ObterOuCriarCarrinhoAtivoAsync(empresaId, cancellationToken);
        return Result.Ok(MapearCarrinhoResponse(carrinho));
    }

    /// <summary>
    /// Adiciona ou atualiza item no carrinho
    /// </summary>
    public async Task<Result<CarrinhoResponseDto>> AtualizarCarrinhoAsync(
        Guid empresaId,
        AtualizarCarrinhoRequestDto request,
        CancellationToken cancellationToken = default)
    {
        // Validar produto
        var produto = await _unitOfWork.Produtos.GetByIdAsync(request.ProdutoId, cancellationToken);
        if (produto == null)
            return Result.Fail<CarrinhoResponseDto>("PRODUTO_NAO_ENCONTRADO");

        // Obter ou criar carrinho
        var carrinho = await ObterOuCriarCarrinhoAtivoAsync(empresaId, cancellationToken);

        // Verificar se item já existe no carrinho
        var itemExistente = carrinho.ItensCarrinho
            .FirstOrDefault(i => i.ProdutoId == request.ProdutoId);

        if (itemExistente != null)
        {
            itemExistente.AtualizarQuantidade(request.Quantidade);
        }
        else
        {
            var novoItem = new ItemCarrinho(
                carrinhoId: carrinho.Id,
                produtoId: request.ProdutoId,
                quantidade: request.Quantidade,
                valorUnitario: produto.ValorProduto);

            carrinho.ItensCarrinho.Add(novoItem);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Carrinho atualizado: Empresa={EmpresaId}, Produto={ProdutoId}, Qtd={Qtd}",
            empresaId, request.ProdutoId, request.Quantidade);

        return Result.Ok(MapearCarrinhoResponse(carrinho));
    }

    /// <summary>
    /// Remove item do carrinho
    /// </summary>
    public async Task<Result<CarrinhoResponseDto>> RemoverItemCarrinhoAsync(
        Guid empresaId,
        Guid produtoId,
        CancellationToken cancellationToken = default)
    {
        var carrinho = await ObterOuCriarCarrinhoAtivoAsync(empresaId, cancellationToken);

        var item = carrinho.ItensCarrinho.FirstOrDefault(i => i.ProdutoId == produtoId);
        if (item != null)
        {
            carrinho.ItensCarrinho.Remove(item);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return Result.Ok(MapearCarrinhoResponse(carrinho));
    }

    /// <summary>
    /// Finaliza carrinho e cria pedido
    /// </summary>
    public async Task<Result<PedidoResponseDto>> FinalizarCarrinhoAsync(
        Guid empresaId,
        CancellationToken cancellationToken = default)
    {
        var carrinho = await ObterOuCriarCarrinhoAtivoAsync(empresaId, cancellationToken);

        if (!carrinho.ItensCarrinho.Any())
            return Result.Fail<PedidoResponseDto>("CARRINHO_VAZIO");

        return await _unitOfWork.ExecuteInTransactionAsync(async (ct) =>
        {
            // Criar pedido
            var numeroPedido = GerarNumeroPedido();
            var pedido = new Pedido(
                numeroPedido: numeroPedido,
                carrinhoId: carrinho.Id,
                empresaId: empresaId);

            await _unitOfWork.Pedidos.AddAsync(pedido, ct);

            // Transferir itens do carrinho para o pedido
            foreach (var itemCarrinho in carrinho.ItensCarrinho)
            {
                var itemPedido = new ItemPedido(
                    pedidoId: pedido.Id,
                    produtoId: itemCarrinho.ProdutoId,
                    quantidade: itemCarrinho.Quantidade,
                    valorUnitario: itemCarrinho.ValorUnitario);

                pedido.ItensPedido.Add(itemPedido);
            }

            // Finalizar carrinho
            carrinho.Finalizar();

            await _unitOfWork.SaveChangesAsync(ct);

            _logger.LogInformation("Pedido criado: Numero={Numero}, Empresa={EmpresaId}", numeroPedido, empresaId);

            return Result.Ok(MapearPedidoResponse(pedido));
        }, cancellationToken);
    }

    /// <summary>
    /// Lista pedidos por CNPJ
    /// </summary>
    public async Task<Result<IEnumerable<PedidoResponseDto>>> ListarPedidosAsync(
        string cnpj,
        CancellationToken cancellationToken = default)
    {
        var cnpjLimpo = cnpj?.Replace(".", "").Replace("-", "").Replace("/", "") ?? "";
        var empresa = await _unitOfWork.Empresas.GetByCnpjAsync(cnpjLimpo, cancellationToken);

        if (empresa == null)
            return Result.Ok(Enumerable.Empty<PedidoResponseDto>());

        var pedidos = await _unitOfWork.Pedidos
            .FindAsync(p => p.EmpresaId == empresa.Id, cancellationToken: cancellationToken);

        var dtos = pedidos.Select(MapearPedidoResponse);
        return Result.Ok(dtos);
    }

    /// <summary>
    /// Obtém detalhes do pedido
    /// </summary>
    public async Task<Result<PedidoResponseDto>> ObterPedidoAsync(
        Guid pedidoId,
        CancellationToken cancellationToken = default)
    {
        var pedido = await _unitOfWork.Pedidos.GetByIdAsync(pedidoId, cancellationToken);
        if (pedido == null)
            return Result.Fail<PedidoResponseDto>("PEDIDO_NAO_ENCONTRADO");

        return Result.Ok(MapearPedidoResponse(pedido));
    }

    /// <summary>
    /// Realiza checkout no Asaas (MOCK)
    /// </summary>
    public async Task<Result<CheckoutResponseDto>> RealizarCheckoutAsync(
        Guid pedidoId,
        CancellationToken cancellationToken = default)
    {
        var pedido = await _unitOfWork.Pedidos.GetByIdAsync(pedidoId, cancellationToken);
        if (pedido == null)
            return Result.Fail<CheckoutResponseDto>("PEDIDO_NAO_ENCONTRADO");

        // MOCK: Simular checkout Asaas
        var idCheckoutMock = $"chk_{Guid.NewGuid():N}";
        var linkMock = $"https://sandbox.asaas.com/checkout/{idCheckoutMock}";

        pedido.ConfirmarPagamento(idCheckoutMock);
        pedido.AtualizarStatus(StatusPedidoEnum.AguardandoPagamento);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Checkout mock realizado: Pedido={PedidoId}, Checkout={CheckoutId}", pedidoId, idCheckoutMock);

        return Result.Ok(new CheckoutResponseDto
        {
            IdCheckout = idCheckoutMock,
            LinkPagamento = linkMock,
            Status = "AGUARDANDO_PAGAMENTO"
        });
    }

    /// <summary>
    /// Processa webhook do Asaas
    /// </summary>
    public async Task<Result> ProcessarWebhookAsync(
        AsaasWebhookRequestDto request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Webhook Asaas recebido: Event={Event}, PaymentId={PaymentId}",
            request.Event, request.Payment?.Id);

        if (request.Payment == null)
            return Result.Fail("PAYMENT_DATA_MISSING");

        // Buscar pedido pelo external reference
        var pedidos = await _unitOfWork.Pedidos
            .FindAsync(p => p.IdCheckout == request.Payment.Id, cancellationToken: cancellationToken);
        var pedido = pedidos.FirstOrDefault();

        if (pedido == null)
            return Result.Fail("PEDIDO_NAO_ENCONTRADO");

        switch (request.Event)
        {
            case "PAYMENT_CONFIRMED":
            case "PAYMENT_RECEIVED":
                await ProcessarPagamentoConfirmado(pedido, request.Payment, cancellationToken);
                break;

            case "PAYMENT_REFUNDED":
                await ProcessarEstorno(pedido, cancellationToken);
                break;

            case "PAYMENT_REFUSED":
                pedido.AtualizarStatus(StatusPedidoEnum.PagamentoRecusado);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                break;
        }

        return Result.Ok();
    }

    /// <summary>
    /// Obtém saldo de créditos da empresa
    /// </summary>
    public async Task<Result<CreditosResponseDto>> ObterCreditosAsync(
        Guid empresaId,
        CancellationToken cancellationToken = default)
    {
        var empresa = await _unitOfWork.Empresas.GetByIdAsync(empresaId, cancellationToken);
        if (empresa == null)
            return Result.Fail<CreditosResponseDto>("EMPRESA_NAO_ENCONTRADA");

        var creditos = await _unitOfWork.CreditosEmpresas
            .FindAsync(c => c.EmpresaId == empresaId, cancellationToken: cancellationToken);

        var creditosList = creditos.ToList();
        var totalCreditos = creditosList.Sum(c => c.Creditos);

        return Result.Ok(new CreditosResponseDto
        {
            EmpresaId = empresaId,
            EmpresaNome = empresa.Nome,
            TotalCreditos = totalCreditos,
            CreditosDisponiveis = totalCreditos, // TODO: Calcular usados
            CreditosPorProduto = creditosList.Select(c => new CreditoPorProdutoDto
            {
                ProdutoId = c.ProdutoId,
                Creditos = c.Creditos
            }).ToList()
        });
    }

    // ==========================================
    // Métodos privados
    // ==========================================

    private async Task<Carrinho> ObterOuCriarCarrinhoAtivoAsync(Guid empresaId, CancellationToken cancellationToken)
    {
        var carrinhos = await _unitOfWork.Carrinhos
            .FindAsync(c => c.EmpresaId == empresaId && c.Status == StatusCarrinhoEnum.Ativo,
                cancellationToken: cancellationToken);

        var carrinho = carrinhos.FirstOrDefault();

        if (carrinho == null)
        {
            carrinho = new Carrinho(empresaId);
            await _unitOfWork.Carrinhos.AddAsync(carrinho, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return carrinho;
    }

    private async Task ProcessarPagamentoConfirmado(Pedido pedido, PaymentData payment, CancellationToken cancellationToken)
    {
        pedido.AtualizarStatus(StatusPedidoEnum.PagamentoConfirmado);

        // Inserir créditos para a empresa
        foreach (var item in pedido.ItensPedido)
        {
            var produto = await _unitOfWork.Produtos.GetByIdAsync(item.ProdutoId, cancellationToken);
            if (produto == null) continue;

            var creditosExistentes = await _unitOfWork.CreditosEmpresas
                .FindSingleAsync(c => c.EmpresaId == pedido.EmpresaId && c.ProdutoId == item.ProdutoId,
                    cancellationToken: cancellationToken);

            if (creditosExistentes != null)
            {
                creditosExistentes.AdicionarCreditos(item.Quantidade * produto.QuantidadeCreditoPorVaga);
            }
            else
            {
                var novoCredito = new CreditosEmpresa(
                    empresaId: pedido.EmpresaId,
                    produtoId: item.ProdutoId,
                    creditos: item.Quantidade * produto.QuantidadeCreditoPorVaga);

                await _unitOfWork.CreditosEmpresas.AddAsync(novoCredito, cancellationToken);
            }
        }

        pedido.MarcarCreditosInseridos();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Pagamento confirmado e créditos inseridos: Pedido={PedidoId}", pedido.Id);
    }

    private async Task ProcessarEstorno(Pedido pedido, CancellationToken cancellationToken)
    {
        pedido.AtualizarStatus(StatusPedidoEnum.PagamentoEstornado);
        // TODO: Remover créditos estornados
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Pagamento estornado: Pedido={PedidoId}", pedido.Id);
    }

    private static int GerarNumeroPedido()
    {
        return int.Parse(DateTime.UtcNow.ToString("yyyyMMdd") + new Random().Next(1000, 9999).ToString());
    }

    private static CarrinhoResponseDto MapearCarrinhoResponse(Carrinho carrinho)
    {
        var valorTotal = carrinho.CalcularTotal();

        return new CarrinhoResponseDto
        {
            Id = carrinho.Id,
            EmpresaId = carrinho.EmpresaId,
            Status = carrinho.Status.ToString(),
            Itens = carrinho.ItensCarrinho.Select(i => new ItemCarrinhoResponseDto
            {
                Id = i.Id,
                ProdutoId = i.ProdutoId,
                NomeProduto = i.Produto?.NomeProduto ?? "",
                Quantidade = i.Quantidade,
                ValorUnitario = i.ValorUnitario,
                ValorTotal = i.Quantidade * i.ValorUnitario
            }).ToList(),
            ValorTotal = valorTotal,
            ValorFinal = valorTotal
        };
    }

    private static PedidoResponseDto MapearPedidoResponse(Pedido pedido)
    {
        var valorTotal = pedido.ItensPedido.Sum(i => i.Quantidade * i.ValorUnitario);

        return new PedidoResponseDto
        {
            Id = pedido.Id,
            NumeroPedido = pedido.NumeroPedido,
            EmpresaId = pedido.EmpresaId,
            EmpresaNome = pedido.Empresa?.Nome ?? "",
            Status = pedido.Status.ToString(),
            ValorTotal = valorTotal,
            ValorFinal = valorTotal,
            IdCheckout = pedido.IdCheckout,
            CreditosInseridos = pedido.CreditosInseridos,
            CreatedAt = pedido.CreatedAt,
            Itens = pedido.ItensPedido.Select(i => new ItemPedidoResponseDto
            {
                ProdutoId = i.ProdutoId,
                NomeProduto = i.Produto?.NomeProduto ?? "",
                Quantidade = i.Quantidade,
                ValorUnitario = i.ValorUnitario,
                ValorTotal = i.Quantidade * i.ValorUnitario
            }).ToList()
        };
    }
}
