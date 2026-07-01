using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace TalentBridge.Api.Configurations;

/// <summary>
/// Configuração de Health Checks para monitoramento da aplicação
/// </summary>
public static class HealthChecksConfiguration
{
    /// <summary>
    /// Adiciona serviços de Health Check ao container de DI
    /// </summary>
    public static IServiceCollection AddHealthChecksConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddHealthChecks()
            .AddNpgSql(
                connectionString: connectionString!,
                name: "PostgreSQL",
                tags: new[] { "database", "postgres" })
            .AddUrlGroup(
                new Uri("https://viacep.com.br"),
                name: "ViaCEP API",
                tags: new[] { "external", "cep" })
            .AddCheck("Self", () =>
                Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy(),
                tags: new[] { "api" })
            .AddCheck("Memory", () =>
                        {
                            var memory = GC.GetTotalMemory(false);
                            var threshold = 1024 * 1024 * 500; // 500MB
                            return memory < threshold
                                ? Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy($"Uso de memória: {memory / 1024 / 1024}MB")
                                : Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Degraded($"Alto uso de memória: {memory / 1024 / 1024}MB");
                        },
                tags: new[] { "system" });

        return services;
    }

    /// <summary>
    /// Mapeia os endpoints de Health Check
    /// </summary>
    public static IEndpointRouteBuilder MapHealthChecksConfiguration(
        this IEndpointRouteBuilder app)
    {
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("api")
        });

        app.MapHealthChecks("/health/database", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("database")
        });

        return app;
    }
}
