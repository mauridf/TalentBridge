using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class CursoConfiguration : IEntityTypeConfiguration<Curso>
{
    public void Configure(EntityTypeBuilder<Curso> builder)
    {
        builder.ToTable("Cursos");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.NomeCurso)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasOne(c => c.PerfilProfissional)
            .WithMany(pp => pp.Cursos)
            .HasForeignKey(c => c.PerfilProfissionalId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Vaga)
            .WithMany()
            .HasForeignKey(c => c.VagaId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(c => c.PerfilProfissionalId)
            .HasDatabaseName("IX_Cursos_PerfilProfissionalId");

        builder.HasIndex(c => c.VagaId)
            .HasDatabaseName("IX_Cursos_VagaId");
    }
}
