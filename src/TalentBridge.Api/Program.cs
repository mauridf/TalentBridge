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
    builder.Services.AddScoped<ILandingPageService, LandingPageService>();
    builder.Services.AddScoped<IDominioService, DominioService>();
    builder.Services.AddScoped<ViaCepService>();

    // HttpClient para ViaCepService
    builder.Services.AddHttpClient<ViaCepService>();

    // Validators
    builder.Services.AddValidatorsFromAssembly(typeof(CandidatoService).Assembly);

    // CORS
    builder.Services.AddCorsConfiguration(builder.Configuration);

    // Configurar OpenAPI com informações da API
    builder.Services.AddOpenApi(options =>
    {
        options.AddDocumentTransformer((document, context, cancellationToken) =>
        {
            document.Info.Title = "TalentBridge API";
            document.Info.Version = "v1";
            document.Info.Description = @"
                                        ## TalentBridge - Plataforma de Recrutamento e Seleção

                                        API completa para conectar **candidatos** a **vagas de emprego**.

                                        ### Funcionalidades Principais:
                                        - 🔐 Autenticação JWT com multi-perfil e multi-empresa
                                        - 👤 Gestão de candidatos com perfil pessoal e profissional
                                        - 🏢 Gestão de empresas, gestores e recrutadores
                                        - 📋 Publicação e busca de vagas com filtros avançados
                                        - 📝 Candidaturas e processo seletivo
                                        - 🧠 Teste comportamental Big Five
                                        - 💰 Sistema de créditos e pagamentos
                                        - 📊 Dashboards com métricas
                                        - 🌐 Landing pages públicas

                                        ### Autenticação:
                                        Use o endpoint `/Usuario/Autenticar` para obter um token JWT.
                                        Envie o token no header: `Authorization: Bearer {token}`
                                        ";
            document.Info.Contact = new()
            {
                Name = "TalentBridge Team",
                Email = "contato@talentbridge.com.br"
            };
            return Task.CompletedTask;
        });
    });

    // OpenAPI + Scalar
    builder.Services.AddOpenApi();

    // Health Checks
    builder.Services.AddHealthChecksConfiguration(builder.Configuration);

    // DbUpInitializer
    builder.Services.AddSingleton<DbUpInitializer>();

    // HttpClient
    builder.Services.AddHttpClient();

    // Rate Limiting
    builder.Services.AddRateLimitingConfiguration();

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

    // Documentação da API (Development only)
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference(options =>
        {
            options.Title = "TalentBridge API";
            options.Theme = Scalar.AspNetCore.ScalarTheme.BluePlanet;
            options.DefaultHttpClient = new(Scalar.AspNetCore.ScalarTarget.CSharp, Scalar.AspNetCore.ScalarClient.HttpClient);
            options.ShowSidebar = true;
            options.HideDownloadButton = false;
            options.HideTestRequestButton = false;
        });
        Log.Information("📚 Scalar disponível em /scalar/v1");
    }

    app.UseCors("AllowSpecificOrigins");

    app.UseRateLimiter();

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
