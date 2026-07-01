using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TalentBridge.Infrastructure.Data;

namespace TalentBridge.Infrastructure.Extensions;

public static class DbUpExtensions
{
    public static IApplicationBuilder UseDbUpMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<DbUpInitializer>>();

        try
        {
            var dbUpInitializer = scope.ServiceProvider.GetRequiredService<DbUpInitializer>();
            dbUpInitializer.ExecuteMigrations();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "❌ Erro crítico ao executar migrations. A aplicação será encerrada.");
            throw;
        }

        return app;
    }
}
