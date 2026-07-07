using FluentResults;
using TalentBridge.Application.DTOs.Segmento;

namespace TalentBridge.Application.Interfaces;

public interface ISegmentoService
{
    Task<Result<IEnumerable<SegmentoResponseDto>>> ListarTodosAsync(CancellationToken cancellationToken = default);
}
