using DbUp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TalentBridge.Infrastructure.Data;

public class DbUpInitializer
{
    private readonly string _connectionString;
    private readonly ILogger<DbUpInitializer> _logger;
    private readonly string _scriptsPath;

    public DbUpInitializer(IConfiguration configuration, ILogger<DbUpInitializer> logger)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' não encontrada.");
        _logger = logger;

        // Caminho relativo à raiz do projeto (executando da pasta Api)
        _scriptsPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "..", "..", "..", "..", "..", "db", "migrations");

        // Se não encontrar, tenta caminho alternativo (Docker/publish)
        if (!Directory.Exists(_scriptsPath))
        {
            _scriptsPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, "db", "migrations");
        }

        // Normalizar caminho
        _scriptsPath = Path.GetFullPath(_scriptsPath);
    }

    public bool ExecuteMigrations()
    {
        _logger.LogInformation("🔧 Iniciando execução de migrations...");
        _logger.LogInformation("📁 Caminho dos scripts: {ScriptsPath}", _scriptsPath);

        if (!Directory.Exists(_scriptsPath))
        {
            _logger.LogError("❌ Diretório de migrations não encontrado: {ScriptsPath}", _scriptsPath);
            _logger.LogInformation("🔍 BaseDirectory: {BaseDir}", AppDomain.CurrentDomain.BaseDirectory);
            return false;
        }

        var scripts = Directory.GetFiles(_scriptsPath, "*.sql")
            .OrderBy(f => f)
            .ToList();

        if (scripts.Count == 0)
        {
            _logger.LogWarning("⚠️ Nenhum arquivo .sql encontrado em: {ScriptsPath}", _scriptsPath);
            return true;
        }

        _logger.LogInformation("📝 {Count} scripts encontrados", scripts.Count);
        foreach (var script in scripts)
        {
            _logger.LogInformation("  → {ScriptName}", Path.GetFileName(script));
        }

        // Garantir que o banco de dados existe
        EnsureDatabase.For.PostgresqlDatabase(_connectionString);

        // Hash BCrypt da senha do admin (Admin@123, cost factor 11)
        var adminSenhaHash = "$2a$11$HUKuFbJ5FTtwVQ0te7h3nOVaP.6JAALAekY18Hvf3XGLrNxWAxj2a";

        // Configurar o DbUp com scripts do sistema de arquivos
        var upgrader = DeployChanges.To
            .PostgresqlDatabase(_connectionString)
            .WithScriptsFromFileSystem(_scriptsPath)
            .WithTransactionPerScript()
            .LogToConsole()
            .WithVariable("AdminSenhaHash", adminSenhaHash)
            .Build();

        // Verificar se há migrations pendentes
        if (!upgrader.IsUpgradeRequired())
        {
            _logger.LogInformation("✅ Nenhuma migration pendente.");
            return true;
        }

        // Obter scripts pendentes
        var scriptsToExecute = upgrader.GetScriptsToExecute();
        _logger.LogInformation("🔄 {Count} scripts pendentes para execução", scriptsToExecute.Count);
        foreach (var script in scriptsToExecute)
        {
            _logger.LogInformation("  → {ScriptName}", script.Name);
        }

        // Executar migrations
        var result = upgrader.PerformUpgrade();

        if (result.Successful)
        {
            _logger.LogInformation("✅ Migrations executadas com sucesso!");
            return true;
        }

        _logger.LogError(result.Error, "❌ Erro ao executar migrations");
        return false;
    }
}
