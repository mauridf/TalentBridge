using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class CompetenciaCandidatoConfiguration : IEntityTypeConfiguration<CompetenciaCandidato>
{
    public void Configure(EntityTypeBuilder<CompetenciaCandidato> builder)
    {
        builder.ToTable("CompetenciasCandidatos");

        builder.HasKey(cc => cc.Id);

        builder.Property(cc => cc.Nivel)
            .IsRequired();

        builder.HasOne(cc => cc.PerfilProfissional)
            .WithMany(pp => pp.CompetenciasCandidatos)
            .HasForeignKey(cc => cc.PerfilProfissionalId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(cc => cc.Competencia)
            .WithMany(c => c.CompetenciasCandidatos)
            .HasForeignKey(cc => cc.CompetenciaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(cc => cc.PerfilProfissionalId)
            .HasDatabaseName("IX_CompetenciasCandidatos_PerfilProfissionalId");

        builder.HasIndex(cc => cc.CompetenciaId)
            .HasDatabaseName("IX_CompetenciasCandidatos_CompetenciaId");
    }
}
