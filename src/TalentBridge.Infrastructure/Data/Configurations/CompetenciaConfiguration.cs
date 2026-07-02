using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class CompetenciaConfiguration : IEntityTypeConfiguration<Competencia>
{
    public void Configure(EntityTypeBuilder<Competencia> builder)
    {
        builder.ToTable("Competencias");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nome)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasOne(c => c.Empresa)
            .WithMany()
            .HasForeignKey(c => c.EmpresaId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(c => c.EmpresaId)
            .HasDatabaseName("IX_Competencias_EmpresaId");
    }
}
