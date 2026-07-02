using TalentBridge.Application.DTOs.Common;

namespace TalentBridge.Application.Interfaces;

public class AnaliseBigFiveResponseDto
{
    public int PontuacaoCompatibilidade { get; set; }
    public string EstiloComportamental { get; set; } = string.Empty;
    public string Atributos { get; set; } = string.Empty;
    public string PontosCriticos { get; set; } = string.Empty;
    public int Score { get; set; }
    public string AnaliseIA { get; set; } = string.Empty;
}

public interface IAnaliseComportamentalService
{
    Task<ResultadoDto<AnaliseBigFiveResponseDto>> AnalisarCompatibilidade(Guid candidatoId, Guid vagaId);
    Task<ResultadoDto<AnaliseBigFiveResponseDto>> GerarRelatorioComportamental(Guid candidatoId);
}
