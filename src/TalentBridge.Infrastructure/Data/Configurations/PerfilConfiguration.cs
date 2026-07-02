using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class PerfilConfiguration : IEntityTypeConfiguration<Perfil>
{
    public void Configure(EntityTypeBuilder<Perfil> builder)
    {
        builder.ToTable("Perfils");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Codigo)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(p => p.Codigo)
            .IsUnique()
            .HasDatabaseName("IX_Perfils_Codigo");

        builder.Property(p => p.Nome)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Descricao)
            .HasMaxLength(500);

        // N:N com Funcionalidade via join table
        builder.HasMany(p => p.Funcionalidades)
            .WithMany(f => f.Perfis)
            .UsingEntity(j => j.ToTable("PerfilFuncionalidades"));
    }
}
