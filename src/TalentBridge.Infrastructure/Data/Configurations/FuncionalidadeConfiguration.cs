using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class FuncionalidadeConfiguration : IEntityTypeConfiguration<Funcionalidade>
{
    public void Configure(EntityTypeBuilder<Funcionalidade> builder)
    {
        builder.ToTable("Funcionalidades");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.Codigo)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(f => f.Codigo)
            .IsUnique()
            .HasDatabaseName("IX_Funcionalidades_Codigo");

        builder.Property(f => f.Descricao)
            .HasMaxLength(500)
            .IsRequired();
    }
}
