using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class CompetenciaVagaConfiguration : IEntityTypeConfiguration<CompetenciaVaga>
{
    public void Configure(EntityTypeBuilder<CompetenciaVaga> builder)
    {
        builder.ToTable("CompetenciasVagas");

        builder.HasKey(cv => cv.Id);

        builder.Property(cv => cv.Nivel)
            .IsRequired();

        builder.Property(cv => cv.Peso)
            .IsRequired();

        builder.HasOne(cv => cv.Vaga)
            .WithMany(v => v.CompetenciasVagas)
            .HasForeignKey(cv => cv.VagaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(cv => cv.Competencia)
            .WithMany(c => c.CompetenciasVagas)
            .HasForeignKey(cv => cv.CompetenciaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(cv => cv.VagaId)
            .HasDatabaseName("IX_CompetenciasVagas_VagaId");

        builder.HasIndex(cv => cv.CompetenciaId)
            .HasDatabaseName("IX_CompetenciasVagas_CompetenciaId");
    }
}
