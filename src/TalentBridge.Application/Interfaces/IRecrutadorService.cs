using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.DTOs.Usuario;

namespace TalentBridge.Application.Interfaces;

public interface IRecrutadorService
{
    Task<ResultadoDto<CriarRecrutadorResponseDto>> CriarAsync(CriarRecrutadorRequestDto request);
    Task<ResultadoDto<CriarRecrutadorResponseDto>> CriarDiretoAsync(CriarRecrutadorDiretoRequestDto request, Guid empresaId);
    Task<ResultadoDto<List<RecrutadorListaDto>>> ListarPorEmpresaAsync(Guid empresaId);
}
