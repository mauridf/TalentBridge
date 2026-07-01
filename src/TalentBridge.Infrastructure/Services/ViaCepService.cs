using System.Net.Http.Json;
using TalentBridge.Application.DTOs.External;

namespace TalentBridge.Infrastructure.Services;

/// <summary>
/// Serviço de consulta de CEP usando ViaCEP
/// </summary>
public class ViaCepService
{
    private readonly HttpClient _httpClient;

    public ViaCepService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Consulta endereço por CEP
    /// </summary>
    public async Task<CepResponseDto?> ConsultarCepAsync(string cep)
    {
        try
        {
            var cepLimpo = cep?.Replace("-", "").Replace(".", "").Trim();
            if (string.IsNullOrWhiteSpace(cepLimpo) || cepLimpo.Length != 8)
                return null;

            var response = await _httpClient.GetAsync($"https://viacep.com.br/ws/{cepLimpo}/json/");

            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadFromJsonAsync<CepResponseDto>();
            return result;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Lista estados brasileiros (mock IBGE)
    /// </summary>
    public List<EstadoDto> ListarEstados()
    {
        return new List<EstadoDto>
        {
            new() { Sigla = "AC", Nome = "Acre" },
            new() { Sigla = "AL", Nome = "Alagoas" },
            new() { Sigla = "AP", Nome = "Amapá" },
            new() { Sigla = "AM", Nome = "Amazonas" },
            new() { Sigla = "BA", Nome = "Bahia" },
            new() { Sigla = "CE", Nome = "Ceará" },
            new() { Sigla = "DF", Nome = "Distrito Federal" },
            new() { Sigla = "ES", Nome = "Espírito Santo" },
            new() { Sigla = "GO", Nome = "Goiás" },
            new() { Sigla = "MA", Nome = "Maranhão" },
            new() { Sigla = "MT", Nome = "Mato Grosso" },
            new() { Sigla = "MS", Nome = "Mato Grosso do Sul" },
            new() { Sigla = "MG", Nome = "Minas Gerais" },
            new() { Sigla = "PA", Nome = "Pará" },
            new() { Sigla = "PB", Nome = "Paraíba" },
            new() { Sigla = "PR", Nome = "Paraná" },
            new() { Sigla = "PE", Nome = "Pernambuco" },
            new() { Sigla = "PI", Nome = "Piauí" },
            new() { Sigla = "RJ", Nome = "Rio de Janeiro" },
            new() { Sigla = "RN", Nome = "Rio Grande do Norte" },
            new() { Sigla = "RS", Nome = "Rio Grande do Sul" },
            new() { Sigla = "RO", Nome = "Rondônia" },
            new() { Sigla = "RR", Nome = "Roraima" },
            new() { Sigla = "SC", Nome = "Santa Catarina" },
            new() { Sigla = "SP", Nome = "São Paulo" },
            new() { Sigla = "SE", Nome = "Sergipe" },
            new() { Sigla = "TO", Nome = "Tocantins" }
        };
    }
}
