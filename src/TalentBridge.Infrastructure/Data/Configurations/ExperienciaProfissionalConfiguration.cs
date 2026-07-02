using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class ExperienciaProfissionalConfiguration : IEntityTypeConfiguration<ExperienciaProfissional>
{
    public void Configure(EntityTypeBuilder<ExperienciaProfissional> builder)
    {
        builder.ToTable("ExperienciasProfissionais");

        builder.HasKey(ep => ep.Id);

        builder.Property(ep => ep.Empresa)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(ep => ep.Posicao)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasOne(ep => ep.PerfilProfissional)
            .WithMany(pp => pp.ExperienciasProfissionais)
            .HasForeignKey(ep => ep.PerfilProfissionalId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(ep => ep.PerfilProfissionalId)
            .HasDatabaseName("IX_ExperienciasProfissionais_PerfilProfissionalId");
    }
}
