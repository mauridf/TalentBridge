using TalentBridge.Application.DTOs.Common;

namespace TalentBridge.Application.Interfaces;

public class FiltrosResponseDto
{
    public List<string> Cidades { get; set; } = new();
    public List<string> Cargos { get; set; } = new();
    public List<string> Regimes { get; set; } = new();
    public List<string> TiposContratacao { get; set; } = new();
}

public interface IFiltroService
{
    Task<ResultadoDto<FiltrosResponseDto>> ObterFiltros();
}
