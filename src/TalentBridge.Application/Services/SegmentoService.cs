using FluentResults;
using TalentBridge.Application.DTOs.Segmento;
using TalentBridge.Application.Interfaces;
using TalentBridge.Domain.Interfaces;

namespace TalentBridge.Application.Services;

public class SegmentoService : ISegmentoService
{
    private readonly IUnitOfWork _unitOfWork;

    public SegmentoService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<SegmentoResponseDto>>> ListarTodosAsync(CancellationToken cancellationToken = default)
    {
        var segmentos = await _unitOfWork.Segmentos
            .GetAllAsync(cancellationToken: cancellationToken);

        var dtos = segmentos.Select(s => new SegmentoResponseDto
        {
            Id = s.Id,
            Nome = s.Nome,
            Descricao = s.Descricao
        });

        return Result.Ok(dtos);
    }
}
