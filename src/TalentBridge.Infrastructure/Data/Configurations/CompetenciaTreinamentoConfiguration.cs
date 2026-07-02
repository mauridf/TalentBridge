using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class CompetenciaTreinamentoConfiguration : IEntityTypeConfiguration<CompetenciaTreinamento>
{
    public void Configure(EntityTypeBuilder<CompetenciaTreinamento> builder)
    {
        builder.ToTable("CompetenciasTreinamentos");

        builder.HasKey(ct => ct.Id);

        builder.Property(ct => ct.Nivel)
            .IsRequired();

        builder.HasOne(ct => ct.Treinamento)
            .WithMany(t => t.CompetenciasTreinamentos)
            .HasForeignKey(ct => ct.TreinamentoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ct => ct.Competencia)
            .WithMany(c => c.CompetenciasTreinamentos)
            .HasForeignKey(ct => ct.CompetenciaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(ct => ct.TreinamentoId)
            .HasDatabaseName("IX_CompetenciasTreinamentos_TreinamentoId");

        builder.HasIndex(ct => ct.CompetenciaId)
            .HasDatabaseName("IX_CompetenciasTreinamentos_CompetenciaId");
    }
}
