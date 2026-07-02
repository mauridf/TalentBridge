using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.DTOs.External;
using TalentBridge.Application.Interfaces;

namespace TalentBridge.Infrastructure.Services.External;

public class GeocodingService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<GeocodingService> _logger;

    public GeocodingService(IHttpClientFactory httpClientFactory, ILogger<GeocodingService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<ResultadoDto<GeocodeResponseDto>> GeocodeEndereco(string cep, string rua, string numero, string bairro, string cidade, string estado)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            var query = $"{rua} {numero}, {bairro}, {cidade}, {estado}, Brasil";
            var encodedQuery = Uri.EscapeDataString(query);

            var response = await client.GetAsync($"https://nominatim.openstreetmap.org/search?q={encodedQuery}&format=json&limit=1");

            if (!response.IsSuccessStatusCode)
                return ResultadoDto<GeocodeResponseDto>.Falha("GEOCODE_ERROR", "Falha ao consultar Nominatim.");

            var results = await response.Content.ReadFromJsonAsync<List<NominatimResult>>();

            if (results is null || results.Count == 0)
                return ResultadoDto<GeocodeResponseDto>.Falha("NOT_FOUND", "Endereço não encontrado.");

            var result = results[0];

            var dto = new GeocodeResponseDto
            {
                Latitude = double.TryParse(result.Lat, out var lat) ? lat : 0,
                Longitude = double.TryParse(result.Lon, out var lon) ? lon : 0
            };

            _logger.LogInformation("Endereço geocodificado: {Lat}, {Lng}", dto.Latitude, dto.Longitude);
            return ResultadoDto<GeocodeResponseDto>.Ok(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao geocodificar endereço: {Cep}, {Rua}", cep, rua);
            return ResultadoDto<GeocodeResponseDto>.Falha("GEOCODE_ERROR", ex.Message);
        }
    }

    private class NominatimResult
    {
        public string Lat { get; set; } = string.Empty;
        public string Lon { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
    }
}
