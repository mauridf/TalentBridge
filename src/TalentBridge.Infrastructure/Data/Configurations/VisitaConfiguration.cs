using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class VisitaConfiguration : IEntityTypeConfiguration<Visita>
{
    public void Configure(EntityTypeBuilder<Visita> builder)
    {
        builder.ToTable("Visitas");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.DataVisita)
            .IsRequired();

        builder.Property(v => v.Ip)
            .HasMaxLength(50);

        builder.Property(v => v.UserAgent)
            .HasMaxLength(500);

        builder.Property(v => v.Origem)
            .HasMaxLength(100);

        builder.HasOne(v => v.Vaga)
            .WithMany()
            .HasForeignKey(v => v.VagaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(v => v.Candidato)
            .WithMany()
            .HasForeignKey(v => v.CandidatoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(v => v.VagaId)
            .HasDatabaseName("IX_Visitas_VagaId");

        builder.HasIndex(v => v.CandidatoId)
            .HasDatabaseName("IX_Visitas_CandidatoId");
    }
}
