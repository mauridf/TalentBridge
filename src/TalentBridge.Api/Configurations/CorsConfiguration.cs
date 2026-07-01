namespace TalentBridge.Api.Configurations;

/// <summary>
/// Configuração de CORS para controle de origens permitidas
/// </summary>
public static class CorsConfiguration
{
    /// <summary>
    /// Adiciona política de CORS ao container de DI
    /// </summary>
    public static IServiceCollection AddCorsConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigins", policy =>
            {
                var allowedOrigins = configuration
                    .GetSection("AllowedOrigins")
                    .Get<string[]>() ?? new[] { "http://localhost:3000", "http://localhost:5173" };

                policy.WithOrigins(allowedOrigins)
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials();
            });
        });

        return services;
    }
}
