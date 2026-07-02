using TalentBridge.Application.DTOs.Common;

namespace TalentBridge.Application.Interfaces;

public interface IGoogleJobsService
{
    Task<ResultadoDto<bool>> PublicarVaga(Guid vagaId);
    Task<ResultadoDto<bool>> RemoverVaga(Guid vagaId);
}
