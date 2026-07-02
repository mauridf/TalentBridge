using TalentBridge.Application.DTOs.Common;

namespace TalentBridge.Application.Interfaces;

public class RastrearVisitaRequestDto
{
    public string? Pagina { get; set; }
    public string? Ip { get; set; }
    public string? UserAgent { get; set; }
    public string? Origem { get; set; }
}

public interface IVisitCounterService
{
    Task<ResultadoDto<bool>> RastrearVisitaAsync(RastrearVisitaRequestDto request);
}
