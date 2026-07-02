using Microsoft.Extensions.Logging;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.DTOs.Credito;
using TalentBridge.Application.Interfaces;
using TalentBridge.Domain.Entities;
using TalentBridge.Domain.Enums;
using TalentBridge.Domain.Interfaces;

namespace TalentBridge.Application.Services;

public class PedidoService : IPedidoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<PedidoService> _logger;

    public PedidoService(IUnitOfWork unitOfWork, ILogger<PedidoService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ResultadoDto<PedidoResponseDto>> ObterPorId(Guid id)
    {
        _logger.LogInformation("Obtendo pedido {PedidoId}", id);

        var pedido = await _unitOfWork.Pedidos.GetByIdAsync(id);
        if (pedido == null)
            return ResultadoDto<PedidoResponseDto>.Falha("PEDIDO_NAO_ENCONTRADO", "Pedido não encontrado.");

        return ResultadoDto<PedidoResponseDto>.Ok(MapearPedido(pedido));
    }

    public async Task<ResultadoDto<List<PedidoResponseDto>>> ListarPorEmpresa(Guid empresaId)
    {
        _logger.LogInformation("Listando pedidos da empresa {EmpresaId}", empresaId);

        var pedidos = await _unitOfWork.Pedidos.FindAsync(p => p.EmpresaId == empresaId);

        var result = pedidos.Select(MapearPedido).ToList();
        return ResultadoDto<List<PedidoResponseDto>>.Ok(result);
    }

    public async Task<ResultadoDto<List<PedidoResponseDto>>> ListarPorCnpj(string cnpj)
    {
        _logger.LogInformation("Listando pedidos pelo CNPJ {Cnpj}", cnpj);

        var cnpjLimpo = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
        var empresas = await _unitOfWork.Empresas.FindAsync(e => e.CNPJ == cnpjLimpo);
        var empresaIds = empresas.Select(e => e.Id).ToHashSet();

        var pedidos = await _unitOfWork.Pedidos.FindAsync(p => empresaIds.Contains(p.EmpresaId));

        var result = pedidos.Select(MapearPedido).ToList();
        return ResultadoDto<List<PedidoResponseDto>>.Ok(result);
    }

    public async Task<ResultadoDto<bool>> AtualizarStatus(Guid id, StatusPedidoEnum novoStatus)
    {
        _logger.LogInformation("Atualizando status do pedido {PedidoId} para {NovoStatus}", id, novoStatus);

        var pedido = await _unitOfWork.Pedidos.GetByIdAsync(id);
        if (pedido == null)
            return ResultadoDto<bool>.Falha("PEDIDO_NAO_ENCONTRADO", "Pedido não encontrado.");

        pedido.AtualizarStatus(novoStatus);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Status do pedido {PedidoId} atualizado para {NovoStatus}", id, novoStatus);

        return ResultadoDto<bool>.Ok(true);
    }

    private static PedidoResponseDto MapearPedido(Pedido pedido)
    {
        return new PedidoResponseDto
        {
            Id = pedido.Id,
            NumeroPedido = pedido.NumeroPedido,
            EmpresaId = pedido.EmpresaId,
            EmpresaNome = pedido.Empresa?.Nome ?? string.Empty,
            Status = pedido.Status.ToString(),
            ValorTotal = pedido.ItensPedido?.Sum(i => i.Quantidade * i.ValorUnitario) ?? 0,
            ValorFinal = pedido.ItensPedido?.Sum(i => i.Quantidade * i.ValorUnitario) ?? 0,
            CreditosInseridos = pedido.CreditosInseridos,
            IdCheckout = pedido.IdCheckout,
            CreatedAt = pedido.CreatedAt,
            Itens = pedido.ItensPedido?.Select(i => new ItemPedidoResponseDto
            {
                ProdutoId = i.ProdutoId,
                NomeProduto = i.Produto?.NomeProduto ?? string.Empty,
                Quantidade = i.Quantidade,
                ValorUnitario = i.ValorUnitario,
                ValorTotal = i.Quantidade * i.ValorUnitario
            }).ToList() ?? new List<ItemPedidoResponseDto>()
        };
    }
}
