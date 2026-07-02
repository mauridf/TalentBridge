using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.DTOs.External;

namespace TalentBridge.Application.Interfaces;

public interface IExternalService
{
    Task<ResultadoDto<CepResponseDto>> ConsultarCep(string cep);
    Task<ResultadoDto<List<EstadoDto>>> ListarEstados();
    Task<ResultadoDto<List<MunicipioDto>>> ListarMunicipios(string uf);
    Task<ResultadoDto<GeocodeResponseDto>> GeocodeEndereco(string cep, string rua, string numero, string bairro, string cidade, string estado);
}

public class GeocodeResponseDto
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
