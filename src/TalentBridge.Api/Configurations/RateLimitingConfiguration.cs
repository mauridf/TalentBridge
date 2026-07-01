using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace TalentBridge.Api.Configurations;

/// <summary>
/// Configuração de rate limiting para proteção contra abusos
/// </summary>
public static class RateLimitingConfiguration
{
    /// <summary>
    /// Adiciona rate limiting ao container de DI
    /// </summary>
    public static IServiceCollection AddRateLimitingConfiguration(
        this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            // Política global: 100 requisições por minuto
            options.AddFixedWindowLimiter("Global", config =>
            {
                config.PermitLimit = 100;
                config.Window = TimeSpan.FromMinutes(1);
                config.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                config.QueueLimit = 10;
            });

            // Política para autenticação: 10 tentativas por minuto
            options.AddFixedWindowLimiter("Auth", config =>
            {
                config.PermitLimit = 10;
                config.Window = TimeSpan.FromMinutes(1);
                config.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                config.QueueLimit = 0;
            });

            // Política para API pública: 30 requisições por minuto
            options.AddFixedWindowLimiter("Public", config =>
            {
                config.PermitLimit = 30;
                config.Window = TimeSpan.FromMinutes(1);
                config.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                config.QueueLimit = 5;
            });

            options.RejectionStatusCode = 429;
        });

        return services;
    }
}
