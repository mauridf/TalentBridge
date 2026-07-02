using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TalentBridge.Domain.Interfaces;
using TalentBridge.Domain.Interfaces.Repositories;
using TalentBridge.Infrastructure.Data;
using TalentBridge.Infrastructure.Repositories;
using TalentBridge.Infrastructure.Services;
using TalentBridge.Infrastructure.Services.Email;
using TalentBridge.Infrastructure.Services.Storage;
using TalentBridge.Infrastructure.Services.Payment;
using TalentBridge.Infrastructure.Services.IA;
using TalentBridge.Infrastructure.Services.External;

namespace TalentBridge.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
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

            if (bool.Parse(configuration["Logging:EnableEfCoreSensitiveDataLogging"] ?? "false"))
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }
        });

        services.AddScoped<IUnitOfWork, Data.UnitOfWork>();

        // Repositories
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<ICandidatoRepository, CandidatoRepository>();
        services.AddScoped<IEmpresaRepository, EmpresaRepository>();
        services.AddScoped<IVagaRepository, VagaRepository>();
        services.AddScoped<ICandidaturaRepository, CandidaturaRepository>();

        return services;
    }

    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<SmtpSettings>(configuration.GetSection("Email:Smtp"));
        services.Configure<MinioSettings>(configuration.GetSection("MinIO"));
        services.Configure<AsaasSettings>(configuration.GetSection("Asaas"));
        services.Configure<OpenAiSettings>(configuration.GetSection("OpenAI"));

        services.AddScoped<EmailService>();
        services.AddScoped<MinioStorageService>();
        services.AddScoped<AsaasPaymentService>();
        services.AddScoped<AnaliseComportamentalService>();
        services.AddScoped<GeocodingService>();
        services.AddScoped<IBGEService>();
        services.AddScoped<WhatsAppIntegrationService>();
        services.AddScoped<GoogleJobsService>();
        services.AddScoped<ViaCepService>();

        return services;
    }
}
