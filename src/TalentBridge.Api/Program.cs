using FluentValidation;
using Scalar.AspNetCore;
using Serilog;
using TalentBridge.Domain.Configurations;
using TalentBridge.Api.Middlewares;
using TalentBridge.Application.Interfaces;
using TalentBridge.Application.Services;
using TalentBridge.Infrastructure.Data;
using TalentBridge.Infrastructure.Extensions;
using TalentBridge.Infrastructure.Services;
using TalentBridge.Api.Configurations;
using TalentBridge.Application.Validators;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. Configurar Serilog
// ==========================================
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

try
{
    Log.Information("🚀 Iniciando TalentBridge API...");
    Log.Information("📅 Ambiente: {Environment}", builder.Environment.EnvironmentName);

    // ==========================================
    // 2. Adicionar serviços
    // ==========================================

    // Controllers
    builder.Services.AddControllers(options =>
    {
        options.SuppressAsyncSuffixInActionNames = false;
    });

    // Configurações tipadas
    builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

    // Infraestrutura (DbContext, UnitOfWork, Repositories)
    builder.Services.AddInfrastructure(builder.Configuration);

    // Autenticação JWT
    builder.Services.AddJwtAuthentication(builder.Configuration);

    // Serviços de aplicação
    builder.Services.AddScoped<ITokenService, TokenService>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<ICandidatoService, CandidatoService>();
    builder.Services.AddScoped<IEmpresaService, EmpresaService>();
    builder.Services.AddScoped<IVagaService, VagaService>();
    builder.Services.AddScoped<ICandidaturaService, CandidaturaService>();
    builder.Services.AddScoped<ICreditoService, CreditoService>();
    builder.Services.AddScoped<IDashboardService, DashboardService>();

    // Validators
    builder.Services.AddValidatorsFromAssembly(typeof(CandidatoService).Assembly);

    // CORS
    builder.Services.AddCorsConfiguration(builder.Configuration);

    // OpenAPI + Scalar
    builder.Services.AddOpenApi();

    // Health Checks
    builder.Services.AddHealthChecksConfiguration(builder.Configuration);

    // DbUpInitializer
    builder.Services.AddSingleton<DbUpInitializer>();

    // HttpClient
    builder.Services.AddHttpClient();

    var app = builder.Build();

    // ==========================================
    // 3. Pipeline de Middlewares
    // ==========================================

    // Middleware global de exceção
    app.UseMiddleware<GlobalExceptionMiddleware>();

    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} respondeu {StatusCode} em {Elapsed:0.0000}ms";
    });

    // Executar Migrations
    app.UseDbUpMigrations();

    // Documentação da API
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference();
        Log.Information("📚 Scalar disponível em /scalar/v1");
    }

    app.UseCors("AllowSpecificOrigins");

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.MapHealthChecksConfiguration();

    // Página inicial
    app.MapGet("/", () => Results.Ok(new
    {
        Api = "TalentBridge API",
        Version = "1.0.0",
        Status = "Online",
        Environment = app.Environment.EnvironmentName,
        Documentation = app.Environment.IsDevelopment() ? "/scalar/v1" : null,
        Endpoints = new
        {
            Auth = "/Usuario/Autenticar",
            Health = "/health",
            Scalar = "/scalar/v1"
        }
    }));

    Log.Information("✅ TalentBridge API iniciada com sucesso!");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "❌ Falha ao iniciar a aplicação");
}
finally
{
    Log.CloseAndFlush();
}
