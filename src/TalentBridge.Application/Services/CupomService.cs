using Microsoft.Extensions.Logging;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.Interfaces;
using TalentBridge.Domain.Entities;
using TalentBridge.Domain.Enums;
using TalentBridge.Domain.Interfaces;

namespace TalentBridge.Application.Services;

public class CupomService : ICupomService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CupomService> _logger;

    public CupomService(IUnitOfWork unitOfWork, ILogger<CupomService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ResultadoDto<CupomResponseDto>> CriarCupom(CriarCupomRequestDto request)
    {
        _logger.LogInformation("Criando cupom: {Nome}", request.Nome);

        if (request.PercentualDesconto < 0 || request.PercentualDesconto > 100)
            return ResultadoDto<CupomResponseDto>.Falha("PERCENTUAL_INVALIDO", "Percentual de desconto deve estar entre 0 e 100.");

        if (request.DataValidade <= DateTime.UtcNow)
            return ResultadoDto<CupomResponseDto>.Falha("DATA_INVALIDA", "Data de validade deve ser futura.");

        var existe = await _unitOfWork.Cupons.ExistsAsync(c => c.Nome == request.Nome.ToUpperInvariant().Trim());
        if (existe)
            return ResultadoDto<CupomResponseDto>.Falha("CUPOM_EXISTENTE", "Já existe um cupom com este nome.");

        var cupom = new Cupom(request.Nome, request.PercentualDesconto, request.DataValidade, request.ParceiroId);
        await _unitOfWork.Cupons.AddAsync(cupom);
        await _unitOfWork.SaveChangesAsync();

        var parceiroNome = request.ParceiroId.HasValue
            ? (await _unitOfWork.Parceiros.GetByIdAsync(request.ParceiroId.Value))?.Nome
            : null;

        _logger.LogInformation("Cupom {CupomId} criado: {Nome}", cupom.Id, cupom.Nome);

        return ResultadoDto<CupomResponseDto>.Ok(new CupomResponseDto
        {
            Id = cupom.Id,
            Nome = cupom.Nome,
            PercentualDesconto = cupom.PercentualDesconto,
            DataValidade = cupom.DataValidade,
            Status = cupom.Status.ToString(),
            ParceiroNome = parceiroNome
        });
    }

    public async Task<ResultadoDto<CupomResponseDto>> Atualizar(Guid id, CriarCupomRequestDto request)
    {
        _logger.LogInformation("Atualizando cupom {CupomId}", id);

        if (request.PercentualDesconto < 0 || request.PercentualDesconto > 100)
            return ResultadoDto<CupomResponseDto>.Falha("PERCENTUAL_INVALIDO", "Percentual de desconto deve estar entre 0 e 100.");

        var cupom = await _unitOfWork.Cupons.GetByIdAsync(id);
        if (cupom == null)
            return ResultadoDto<CupomResponseDto>.Falha("CUPOM_NAO_ENCONTRADO", "Cupom não encontrado.");

        _unitOfWork.Cupons.Remove(cupom);

        var novoCupom = new Cupom(request.Nome, request.PercentualDesconto, request.DataValidade, request.ParceiroId);
        novoCupom.DefinirId(id);
        await _unitOfWork.Cupons.AddAsync(novoCupom);
        await _unitOfWork.SaveChangesAsync();

        var parceiroNome = request.ParceiroId.HasValue
            ? (await _unitOfWork.Parceiros.GetByIdAsync(request.ParceiroId.Value))?.Nome
            : null;

        return ResultadoDto<CupomResponseDto>.Ok(new CupomResponseDto
        {
            Id = novoCupom.Id,
            Nome = novoCupom.Nome,
            PercentualDesconto = novoCupom.PercentualDesconto,
            DataValidade = novoCupom.DataValidade,
            Status = novoCupom.Status.ToString(),
            ParceiroNome = parceiroNome
        });
    }

    public async Task<ResultadoDto<bool>> Inativar(Guid id)
    {
        _logger.LogInformation("Inativando cupom {CupomId}", id);

        var cupom = await _unitOfWork.Cupons.GetByIdAsync(id);
        if (cupom == null)
            return ResultadoDto<bool>.Falha("CUPOM_NAO_ENCONTRADO", "Cupom não encontrado.");

        _unitOfWork.Cupons.Remove(cupom);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Cupom {CupomId} inativado", id);

        return ResultadoDto<bool>.Ok(true);
    }

    public async Task<ResultadoDto<CupomResponseDto>> BuscarPorCodigo(string codigo)
    {
        _logger.LogInformation("Buscando cupom pelo código: {Codigo}", codigo);

        var cupom = await _unitOfWork.Cupons.FindSingleAsync(c => c.Nome == codigo.ToUpperInvariant().Trim());
        if (cupom == null)
            return ResultadoDto<CupomResponseDto>.Falha("CUPOM_NAO_ENCONTRADO", "Cupom não encontrado.");

        string? parceiroNome = null;
        if (cupom.ParceiroId.HasValue)
        {
            var parceiro = await _unitOfWork.Parceiros.GetByIdAsync(cupom.ParceiroId.Value);
            parceiroNome = parceiro?.Nome;
        }

        return ResultadoDto<CupomResponseDto>.Ok(new CupomResponseDto
        {
            Id = cupom.Id,
            Nome = cupom.Nome,
            PercentualDesconto = cupom.PercentualDesconto,
            DataValidade = cupom.DataValidade,
            Status = cupom.Status.ToString(),
            ParceiroNome = parceiroNome
        });
    }

    public async Task<ResultadoDto<List<CupomResponseDto>>> ListarTodos()
    {
        _logger.LogInformation("Listando todos os cupons");

        var cupons = await _unitOfWork.Cupons.GetAllAsync();

        var result = new List<CupomResponseDto>();
        foreach (var cupom in cupons)
        {
            string? parceiroNome = null;
            if (cupom.ParceiroId.HasValue)
            {
                var parceiro = await _unitOfWork.Parceiros.GetByIdAsync(cupom.ParceiroId.Value);
                parceiroNome = parceiro?.Nome;
            }

            result.Add(new CupomResponseDto
            {
                Id = cupom.Id,
                Nome = cupom.Nome,
                PercentualDesconto = cupom.PercentualDesconto,
                DataValidade = cupom.DataValidade,
                Status = cupom.Status.ToString(),
                ParceiroNome = parceiroNome
            });
        }

        return ResultadoDto<List<CupomResponseDto>>.Ok(result);
    }
}
