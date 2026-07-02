using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.DTOs.Credito;
using TalentBridge.Application.Interfaces;

namespace TalentBridge.Infrastructure.Services.Payment;

public class AsaasSettings
{
    public string ApiKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
    public string WebhookSecret { get; set; } = string.Empty;
}

public class AsaasPaymentService : IAsaasService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AsaasSettings _settings;
    private readonly ILogger<AsaasPaymentService> _logger;

    public AsaasPaymentService(
        IHttpClientFactory httpClientFactory,
        IOptions<AsaasSettings> settings,
        ILogger<AsaasPaymentService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task<ResultadoDto<CheckoutResponseDto>> RealizarCheckout(Guid pedidoId)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("Asaas");
            client.BaseAddress = new Uri(_settings.BaseUrl);
            client.DefaultRequestHeaders.Add("access_token", _settings.ApiKey);

            var payload = new { externalReference = pedidoId.ToString() };

            var response = await client.PostAsJsonAsync("/payments", payload);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                _logger.LogError("Erro AsaAS ao criar checkout. Status: {Status}, Body: {Body}", response.StatusCode, errorBody);
                return ResultadoDto<CheckoutResponseDto>.Falha("ASAAS_ERROR", "Erro ao comunicar com Asaas.");
            }

            var result = await response.Content.ReadFromJsonAsync<AsaasPaymentResponse>();

            var dto = new CheckoutResponseDto
            {
                IdCheckout = result?.Id ?? string.Empty,
                LinkPagamento = result?.InvoiceUrl ?? string.Empty,
                Status = result?.Status ?? string.Empty
            };

            _logger.LogInformation("Checkout Asaas criado: {IdCheckout}", dto.IdCheckout);
            return ResultadoDto<CheckoutResponseDto>.Ok(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao realizar checkout Asaas para pedido {PedidoId}", pedidoId);
            return ResultadoDto<CheckoutResponseDto>.Falha("CHECKOUT_ERROR", ex.Message);
        }
    }

    public async Task<ResultadoDto<bool>> ProcessarWebhook(AsaasWebhookRequestDto webhook)
    {
        try
        {
            _logger.LogInformation("Webhook Asaas recebido: Event={Event}, PaymentId={PaymentId}",
                webhook.Event, webhook.Payment?.Id);

            if (webhook.Payment is null)
                return ResultadoDto<bool>.Falha("WEBHOOK_INVALID", "Payment data not found.");

            return ResultadoDto<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar webhook Asaas");
            return ResultadoDto<bool>.Falha("WEBHOOK_ERROR", ex.Message);
        }
    }

    private class AsaasPaymentResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string InvoiceUrl { get; set; } = string.Empty;
    }
}
