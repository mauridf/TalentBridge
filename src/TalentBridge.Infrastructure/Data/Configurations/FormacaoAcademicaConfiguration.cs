using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class FormacaoAcademicaConfiguration : IEntityTypeConfiguration<FormacaoAcademica>
{
    public void Configure(EntityTypeBuilder<FormacaoAcademica> builder)
    {
        builder.ToTable("FormacoesAcademicas");

        builder.HasKey(fa => fa.Id);

        builder.Property(fa => fa.Grau)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(fa => fa.AreaAtuacao)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasOne(fa => fa.PerfilProfissional)
            .WithMany(pp => pp.FormacoesAcademicas)
            .HasForeignKey(fa => fa.PerfilProfissionalId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(fa => fa.PerfilProfissionalId)
            .HasDatabaseName("IX_FormacoesAcademicas_PerfilProfissionalId");
    }
}
