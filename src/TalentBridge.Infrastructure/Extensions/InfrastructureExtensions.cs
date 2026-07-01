using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TalentBridge.Infrastructure.Data;

namespace TalentBridge.Infrastructure.Extensions;

/// <summary>
/// Extensões para registrar serviços da camada de infraestrutura
/// </summary>
public static class InfrastructureExtensions
{
    /// <summary>
    /// Adiciona os serviços de infraestrutura (DbContext, Repositories, UnitOfWork)
    /// </summary>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Registrar DbContext
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<TalentBridgeDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsAssembly(typeof(TalentBridgeDbContext).Assembly.FullName);
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorCodesToAdd: null);
            });

            // Habilitar logging detalhado em desenvolvimento
            if (bool.Parse(configuration["Logging:EnableEfCoreSensitiveDataLogging"] ?? "false"))
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }
        });

        // Registrar UnitOfWork
        services.AddScoped<Domain.Interfaces.IUnitOfWork, Data.UnitOfWork>();

        return services;
    }
}
