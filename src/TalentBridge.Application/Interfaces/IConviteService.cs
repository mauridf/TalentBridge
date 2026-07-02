using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.DTOs.Usuario;

namespace TalentBridge.Application.Interfaces;

public interface IConviteService
{
    Task<ResultadoDto<ConviteResponseDto>> CriarConvite(CriarConviteRequestDto request, Guid empresaResponsavelId);
    Task<ResultadoDto<ConviteResponseDto>> ValidarToken(string token);
    Task<ResultadoDto<List<ConviteResponseDto>>> ListarPorEmpresa(Guid empresaId);
    Task<ResultadoDto<bool>> Inativar(Guid conviteId);
}
