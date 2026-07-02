using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class ApiLogConfiguration : IEntityTypeConfiguration<ApiLog>
{
    public void Configure(EntityTypeBuilder<ApiLog> builder)
    {
        builder.ToTable("ApiLogs");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Metodo)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(l => l.Rota)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(l => l.Ip)
            .HasMaxLength(50);

        builder.Property(l => l.UserAgent)
            .HasMaxLength(500);

        builder.HasIndex(l => l.CreatedAt);
        builder.HasIndex(l => l.StatusCode);

        // Propriedades de log complexas (armazenadas como JSON ou ignoradas)
        builder.Ignore(l => l.Requisicao);
        builder.Ignore(l => l.Resposta);
    }
}
