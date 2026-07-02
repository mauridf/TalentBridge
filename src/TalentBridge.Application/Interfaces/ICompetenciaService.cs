using TalentBridge.Application.DTOs.Common;

namespace TalentBridge.Application.Interfaces;

public class CriarCompetenciaRequestDto
{
    public string Nome { get; set; } = string.Empty;
    public Guid? EmpresaId { get; set; }
}

public class CompetenciaResponseDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public Guid? EmpresaId { get; set; }
    public string? EmpresaNome { get; set; }
}

public interface ICompetenciaService
{
    Task<ResultadoDto<CompetenciaResponseDto>> CriarCompetencia(CriarCompetenciaRequestDto request);
    Task<ResultadoDto<List<CompetenciaResponseDto>>> Buscar(string? nome, Guid? empresaId);
    Task<ResultadoDto<bool>> Remover(Guid id);
}
