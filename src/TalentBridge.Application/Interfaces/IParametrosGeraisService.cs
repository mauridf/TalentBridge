using TalentBridge.Application.DTOs.Common;

namespace TalentBridge.Application.Interfaces;

public class ParametroResponseDto
{
    public string Chave { get; set; } = string.Empty;
    public string Valor { get; set; } = string.Empty;
    public string? Descricao { get; set; }
}

public class AtualizarParametroRequestDto
{
    public string Valor { get; set; } = string.Empty;
}

public interface IParametrosGeraisService
{
    Task<ResultadoDto<ParametroResponseDto>> Obter(string chave);
    Task<ResultadoDto<List<ParametroResponseDto>>> ListarTodos();
    Task<ResultadoDto<bool>> Atualizar(string chave, string valor);
}
