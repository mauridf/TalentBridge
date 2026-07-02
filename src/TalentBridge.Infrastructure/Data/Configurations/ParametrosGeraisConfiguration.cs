using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class ParametrosGeraisConfiguration : IEntityTypeConfiguration<ParametrosGerais>
{
    public void Configure(EntityTypeBuilder<ParametrosGerais> builder)
    {
        builder.ToTable("ParametrosGerais");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Chave)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Valor)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(p => p.Descricao)
            .HasMaxLength(500);

        builder.HasIndex(p => p.Chave)
            .IsUnique();
    }
}
