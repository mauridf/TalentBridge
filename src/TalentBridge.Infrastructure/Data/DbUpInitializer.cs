using DbUp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace TalentBridge.Infrastructure.Data;

public class DbUpInitializer
{
    private readonly string _connectionString;
    private readonly ILogger<DbUpInitializer> _logger;

    public DbUpInitializer(IConfiguration configuration, ILogger<DbUpInitializer> logger)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' não encontrada.");
        _logger = logger;
    }

    public bool ExecuteMigrations()
    {
        _logger.LogInformation("🔧 Iniciando execução de migrations...");

        // Garantir que o banco de dados existe
        EnsureDatabase.For.PostgresqlDatabase(_connectionString);

        // Configurar o DbUp
        var upgrader = DeployChanges.To
            .PostgresqlDatabase(_connectionString)
            .WithScriptsEmbeddedInAssembly(
                Assembly.GetExecutingAssembly(),
                scriptName => scriptName.StartsWith("TalentBridge.Infrastructure.Migrations."),
                new DbUp.Engine.SqlScriptOptions
                {
                    ScriptType = DbUp.Engine.ScriptType.RunOnce,
                    RunGroupOrder = DbUp.Engine.RunGroupOrder.ByScriptName
                })
            .WithTransactionPerScript()
            .LogToConsole()
            .LogScriptOutput()
            .Build();

        // Verificar se há migrations pendentes
        if (!upgrader.IsUpgradeRequired())
        {
            _logger.LogInformation("✅ Nenhuma migration pendente.");
            return true;
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
