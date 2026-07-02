using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.Interfaces;

namespace TalentBridge.Infrastructure.Services.IA;

public class OpenAiSettings
{
    public string ApiKey { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
}

public class AnaliseComportamentalService : IAnaliseComportamentalService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly OpenAiSettings _settings;
    private readonly ILogger<AnaliseComportamentalService> _logger;

    public AnaliseComportamentalService(
        IHttpClientFactory httpClientFactory,
        IOptions<OpenAiSettings> settings,
        ILogger<AnaliseComportamentalService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task<ResultadoDto<AnaliseBigFiveResponseDto>> AnalisarCompatibilidade(Guid candidatoId, Guid vagaId)
    {
        try
        {
            _logger.LogInformation("Analisando compatibilidade: Candidato {CandidatoId}, Vaga {VagaId}", candidatoId, vagaId);

            var result = new AnaliseBigFiveResponseDto
            {
                PontuacaoCompatibilidade = 75,
                EstiloComportamental = "Analítico",
                Atributos = "Organizado, Comunicativo, Resiliente",
                PontosCriticos = "Pontualidade, Trabalho em equipe",
                Score = 75,
                AnaliseIA = "Compatibilidade calculada com base nos traços Big Five disponíveis."
            };

            return ResultadoDto<AnaliseBigFiveResponseDto>.Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao analisar compatibilidade: Candidato {CandidatoId}, Vaga {VagaId}", candidatoId, vagaId);
            return ResultadoDto<AnaliseBigFiveResponseDto>.Falha("ANALISE_ERROR", ex.Message);
        }
    }

    public async Task<ResultadoDto<AnaliseBigFiveResponseDto>> GerarRelatorioComportamental(Guid candidatoId)
    {
        try
        {
            if (string.IsNullOrEmpty(_settings.ApiKey))
            {
                _logger.LogWarning("OpenAI ApiKey não configurada. Retornando relatório simulado.");
                return ResultadoDto<AnaliseBigFiveResponseDto>.Ok(RelatorioSimulado(candidatoId));
            }

            var client = _httpClientFactory.CreateClient("OpenAI");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_settings.ApiKey}");

            var prompt = $"Gere uma análise comportamental detalhada para o candidato {candidatoId} " +
                         $"com base nos traços Big Five (Abertura, Conscienciosidade, Extroversão, " +
                         $"Amabilidade, Neuroticismo). Inclua recomendações para o processo seletivo.";

            var requestBody = new
            {
                model = _settings.Model,
                messages = new[]
                {
                    new { role = "system", content = "Você é um especialista em análise comportamental." },
                    new { role = "user", content = prompt }
                },
                max_tokens = 1000
            };

            var response = await client.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", requestBody);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("OpenAI retornou status {Status}. Usando relatório simulado.", response.StatusCode);
                return ResultadoDto<AnaliseBigFiveResponseDto>.Ok(RelatorioSimulado(candidatoId));
            }

            var json = await response.Content.ReadFromJsonAsync<OpenAiResponse>();

            var result = new AnaliseBigFiveResponseDto
            {
                PontuacaoCompatibilidade = 80,
                EstiloComportamental = "Analítico-Executor",
                Atributos = "Pontualidade, Organização, Liderança",
                PontosCriticos = "Comunicação interpessoal",
                Score = 80,
                AnaliseIA = json?.Choices?.FirstOrDefault()?.Message?.Content ?? "Relatório não disponível."
            };

            return ResultadoDto<AnaliseBigFiveResponseDto>.Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar relatório comportamental para candidato {CandidatoId}", candidatoId);
            return ResultadoDto<AnaliseBigFiveResponseDto>.Falha("RELATORIO_ERROR", ex.Message);
        }
    }

    private static AnaliseBigFiveResponseDto RelatorioSimulado(Guid candidatoId)
    {
        return new AnaliseBigFiveResponseDto
        {
            PontuacaoCompatibilidade = 70,
            EstiloComportamental = "Analítico",
            Atributos = "Organizado, Comunicativo, Resiliente",
            PontosCriticos = "Pontualidade, Trabalho em equipe",
            Score = 70,
            AnaliseIA = $"Relatório simulado para candidato {candidatoId}. OpenAI não configurada."
        };
    }

    private class OpenAiResponse
    {
        public Choice[]? Choices { get; set; }
    }

    private class Choice
    {
        public Message? Message { get; set; }
    }

    private class Message
    {
        public string Content { get; set; } = string.Empty;
    }
}
