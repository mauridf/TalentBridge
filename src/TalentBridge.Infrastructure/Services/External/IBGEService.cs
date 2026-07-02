using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.DTOs.External;

namespace TalentBridge.Infrastructure.Services.External;

public class IBGEService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<IBGEService> _logger;

    public IBGEService(IHttpClientFactory httpClientFactory, ILogger<IBGEService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<ResultadoDto<List<EstadoDto>>> ListarEstados()
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://servicodados.ibge.gov.br/api/v1/localidades/estados");

            if (!response.IsSuccessStatusCode)
                return ResultadoDto<List<EstadoDto>>.Falha("IBGE_ERROR", "Falha ao consultar IBGE.");

            var estados = await response.Content.ReadFromJsonAsync<List<IbgeEstadoResponse>>();

            if (estados is null)
                return ResultadoDto<List<EstadoDto>>.Falha("IBGE_ERROR", "Resposta inválida do IBGE.");

            var result = estados
                .OrderBy(e => e.Nome)
                .Select(e => new EstadoDto { Sigla = e.Sigla, Nome = e.Nome })
                .ToList();

            return ResultadoDto<List<EstadoDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar estados do IBGE");
            return ResultadoDto<List<EstadoDto>>.Falha("IBGE_ERROR", ex.Message);
        }
    }

    public async Task<ResultadoDto<List<MunicipioDto>>> ListarMunicipios(string uf)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://servicodados.ibge.gov.br/api/v1/localidades/estados/{uf}/municipios");

            if (!response.IsSuccessStatusCode)
                return ResultadoDto<List<MunicipioDto>>.Falha("IBGE_ERROR", "Falha ao consultar IBGE.");

            var municipios = await response.Content.ReadFromJsonAsync<List<IbgeMunicipioResponse>>();

            if (municipios is null)
                return ResultadoDto<List<MunicipioDto>>.Falha("IBGE_ERROR", "Resposta inválida do IBGE.");

            var result = municipios
                .OrderBy(m => m.Nome)
                .Select(m => new MunicipioDto { Codigo = m.Id, Nome = m.Nome })
                .ToList();

            return ResultadoDto<List<MunicipioDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar municípios do IBGE para UF {Uf}", uf);
            return ResultadoDto<List<MunicipioDto>>.Falha("IBGE_ERROR", ex.Message);
        }
    }

    private class IbgeEstadoResponse
    {
        public string Sigla { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
    }

    private class IbgeMunicipioResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
    }
}
