using FluentResults;
using TalentBridge.Application.Interfaces;
using TalentBridge.Domain.Enums;
using TalentBridge.Domain.Interfaces;

namespace TalentBridge.Application.Services;

/// <summary>
/// Implementação do serviço de domínios
/// </summary>
public class DominioService : IDominioService
{
    private readonly IUnitOfWork _unitOfWork;

    public DominioService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<DominioDto>>> BuscarPorTipoAsync(
        TipoDominioEnum tipo,
        CancellationToken cancellationToken = default)
    {
        var dominios = await _unitOfWork.Dominios
            .FindAsync(d => d.Tipo == tipo && d.Status == StatusComumEnum.Ativo,
                cancellationToken: cancellationToken);

        var dtos = dominios.Select(d => new DominioDto
        {
            Codigo = d.Codigo,
            Descricao = d.Descricao,
            Tipo = d.Tipo.ToString(),
            Ativo = d.Status == StatusComumEnum.Ativo
        });

        return Result.Ok(dtos);
    }

    public async Task<Result<Dictionary<string, List<DominioDto>>>> BuscarTodosAsync(
        CancellationToken cancellationToken = default)
    {
        var todosDominios = await _unitOfWork.Dominios
            .FindAsync(d => d.Status == StatusComumEnum.Ativo,
                cancellationToken: cancellationToken);

        var agrupado = todosDominios
            .GroupBy(d => d.Tipo)
            .ToDictionary(
                g => g.Key.ToString(),
                g => g.Select(d => new DominioDto
                {
                    Codigo = d.Codigo,
                    Descricao = d.Descricao,
                    Tipo = d.Tipo.ToString(),
                    Ativo = true
                }).ToList());

        return Result.Ok(agrupado);
    }
}
