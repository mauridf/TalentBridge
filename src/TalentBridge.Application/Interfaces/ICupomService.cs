using TalentBridge.Application.DTOs.Common;

namespace TalentBridge.Application.Interfaces;

public class CriarCupomRequestDto
{
    public string Nome { get; set; } = string.Empty;
    public int PercentualDesconto { get; set; }
    public DateTime DataValidade { get; set; }
    public Guid? ParceiroId { get; set; }
}

public class CupomResponseDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int PercentualDesconto { get; set; }
    public DateTime DataValidade { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? ParceiroNome { get; set; }
}

public interface ICupomService
{
    Task<ResultadoDto<CupomResponseDto>> CriarCupom(CriarCupomRequestDto request);
    Task<ResultadoDto<CupomResponseDto>> Atualizar(Guid id, CriarCupomRequestDto request);
    Task<ResultadoDto<bool>> Inativar(Guid id);
    Task<ResultadoDto<CupomResponseDto>> BuscarPorCodigo(string codigo);
    Task<ResultadoDto<List<CupomResponseDto>>> ListarTodos();
}
