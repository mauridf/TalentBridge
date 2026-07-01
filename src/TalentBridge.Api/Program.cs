using Serilog;
using TalentBridge.Api.Configurations;
using TalentBridge.Infrastructure.Data;
using TalentBridge.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. Configurar Serilog (primeiro!)
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

    // Configurações tipadas (Strongly Typed Settings)
    builder.Services.Configure<JwtSettings>(
        builder.Configuration.GetSection("JwtSettings"));

    // Configurar CORS
    builder.Services.AddCorsConfiguration(builder.Configuration);

    // Configurar Swagger/OpenAPI + Scalar
    builder.Services.AddOpenApi();

    // Health Checks
    builder.Services.AddHealthChecksConfiguration(builder.Configuration);

    // Registrar DbUpInitializer
    builder.Services.AddSingleton<DbUpInitializer>();

    // Registrar HttpClient para serviços externos
    builder.Services.AddHttpClient();

    var app = builder.Build();

    // ==========================================
    // 3. Executar Migrations (antes dos middlewares)
    // ==========================================
    app.UseDbUpMigrations();

    // ==========================================
    // 4. Pipeline de Middlewares
    // ==========================================

    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} respondeu {StatusCode} em {Elapsed:0.0000}ms";
    });

    // Documentação da API (Development only)
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalar();
        Log.Information("📚 Scalar disponível em /scalar/v1");
    }

    app.UseCors("AllowSpecificOrigins");

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.MapHealthChecksConfiguration();

    // Página inicial informativa
    app.MapGet("/", () => Results.Ok(new
    {
        Api = "TalentBridge API",
        Version = "1.0.0",
        Status = "Online",
        Environment = app.Environment.EnvironmentName,
        Documentation = app.Environment.IsDevelopment() ? "/scalar/v1" : "Not available in production"
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
