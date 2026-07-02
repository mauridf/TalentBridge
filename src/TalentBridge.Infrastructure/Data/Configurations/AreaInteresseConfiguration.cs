using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class AreaInteresseConfiguration : IEntityTypeConfiguration<AreaInteresse>
{
    public void Configure(EntityTypeBuilder<AreaInteresse> builder)
    {
        builder.ToTable("AreasInteresse");

        builder.HasKey(ai => ai.Id);

        builder.Property(ai => ai.Codigo)
            .IsRequired();

        builder.HasIndex(ai => ai.Codigo)
            .IsUnique()
            .HasDatabaseName("IX_AreasInteresse_Codigo");

        builder.Property(ai => ai.Nome)
            .HasMaxLength(100)
            .IsRequired();

        // N:N com PerfilProfissional via join table
        builder.HasMany(ai => ai.PerfisProfissionais)
            .WithMany(pp => pp.AreasInteresse)
            .UsingEntity(j => j.ToTable("AreasInteressePerfisProfissionais"));
    }
}
