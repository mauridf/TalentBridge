using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class CandidaturaConfiguration : IEntityTypeConfiguration<Candidatura>
{
    public void Configure(EntityTypeBuilder<Candidatura> builder)
    {
        builder.ToTable("Candidaturas");

        builder.HasKey(c => c.Id);

        // Relacionamento com Vaga
        builder.HasOne(c => c.Vaga)
            .WithMany(v => v.Candidaturas)
            .HasForeignKey(c => c.VagaId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relacionamento com Candidato
        builder.HasOne(c => c.Candidato)
            .WithMany(cand => cand.Candidaturas)
            .HasForeignKey(c => c.CandidatoId)
            .OnDelete(DeleteBehavior.Restrict);

        // Índice único composto para evitar duplicidade
        builder.HasIndex(c => new { c.VagaId, c.CandidatoId })
            .IsUnique()
            .HasDatabaseName("IX_Candidaturas_VagaId_CandidatoId");

        builder.Property(c => c.Protocolo)
            .HasMaxLength(20);

        builder.Property(c => c.LinkEntrevista)
            .HasMaxLength(500);

        builder.Property(c => c.MeioEntrevista)
            .HasMaxLength(50);
    }
}
