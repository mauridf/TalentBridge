using TalentBridge.Application.DTOs.Common;

namespace TalentBridge.Application.Interfaces;

public class RastrearConversaoRequestDto
{
    public string? Tipo { get; set; }
    public string? Origem { get; set; }
    public string? Identificador { get; set; }
}

public interface IConversionTrackingService
{
    Task<ResultadoDto<bool>> RastrearConversaoAsync(RastrearConversaoRequestDto request);
}
