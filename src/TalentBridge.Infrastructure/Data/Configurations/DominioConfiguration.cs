using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class DominioConfiguration : IEntityTypeConfiguration<Dominio>
{
    public void Configure(EntityTypeBuilder<Dominio> builder)
    {
        builder.ToTable("Dominios");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Codigo)
            .IsRequired();

        builder.Property(d => d.Descricao)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(d => d.Tipo)
            .IsRequired();

        // Índice único composto
        builder.HasIndex(d => new { d.Tipo, d.Codigo })
            .IsUnique()
            .HasDatabaseName("UQ_Dominios_Tipo_Codigo");

        builder.HasIndex(d => d.Tipo)
            .HasDatabaseName("IX_Dominios_Tipo");
    }
}
