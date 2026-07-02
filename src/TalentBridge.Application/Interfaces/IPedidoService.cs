using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.DTOs.Credito;
using TalentBridge.Domain.Enums;

namespace TalentBridge.Application.Interfaces;

public interface IPedidoService
{
    Task<ResultadoDto<PedidoResponseDto>> ObterPorId(Guid id);
    Task<ResultadoDto<List<PedidoResponseDto>>> ListarPorEmpresa(Guid empresaId);
    Task<ResultadoDto<List<PedidoResponseDto>>> ListarPorCnpj(string cnpj);
    Task<ResultadoDto<bool>> AtualizarStatus(Guid id, StatusPedidoEnum novoStatus);
}
