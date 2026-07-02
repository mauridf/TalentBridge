using Microsoft.Extensions.Logging;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.DTOs.External;
using TalentBridge.Application.Interfaces;

namespace TalentBridge.Application.Services;

public class ExternalService : IExternalService
{
    private readonly ILogger<ExternalService> _logger;

    public ExternalService(ILogger<ExternalService> logger)
    {
        _logger = logger;
    }

    public Task<ResultadoDto<CepResponseDto>> ConsultarCep(string cep)
    {
        _logger.LogWarning("ExternalService.ConsultarCep não implementado. CEP: {Cep}", cep);
        throw new NotImplementedException("ExternalService.ConsultarCep ainda não foi implementado.");
    }

    public Task<ResultadoDto<List<EstadoDto>>> ListarEstados()
    {
        _logger.LogWarning("ExternalService.ListarEstados não implementado.");
        throw new NotImplementedException("ExternalService.ListarEstados ainda não foi implementado.");
    }

    public Task<ResultadoDto<List<MunicipioDto>>> ListarMunicipios(string uf)
    {
        _logger.LogWarning("ExternalService.ListarMunicipios não implementado. UF: {Uf}", uf);
        throw new NotImplementedException("ExternalService.ListarMunicipios ainda não foi implementado.");
    }

    public Task<ResultadoDto<GeocodeResponseDto>> GeocodeEndereco(string cep, string rua, string numero, string bairro, string cidade, string estado)
    {
        _logger.LogWarning("ExternalService.GeocodeEndereco não implementado. CEP: {Cep}", cep);
        throw new NotImplementedException("ExternalService.GeocodeEndereco ainda não foi implementado.");
    }
}
