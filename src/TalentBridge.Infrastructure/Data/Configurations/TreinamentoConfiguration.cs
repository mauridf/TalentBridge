using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class TreinamentoConfiguration : IEntityTypeConfiguration<Treinamento>
{
    public void Configure(EntityTypeBuilder<Treinamento> builder)
    {
        builder.ToTable("Treinamentos");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.NomeCurso)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(t => t.Categoria)
            .HasMaxLength(100);

        builder.Property(t => t.Nivel)
            .HasMaxLength(50);

        builder.Property(t => t.Descricao)
            .HasMaxLength(2000);

        builder.Property(t => t.ResultadosAprendizagem)
            .HasMaxLength(2000);

        builder.Property(t => t.Criador)
            .HasMaxLength(100);

        // N:N com Vagas (join table: TreinamentoVagas)
        builder.HasMany(t => t.Vagas)
            .WithMany()
            .UsingEntity(j => j.ToTable("TreinamentoVagas"));

        // 1:N com ModulosCurso
        builder.HasMany(t => t.ModulosCursos)
            .WithOne(m => m.Treinamento)
            .HasForeignKey(m => m.TreinamentoId)
            .OnDelete(DeleteBehavior.Cascade);

        // 1:N com ItensIncluidos
        builder.HasMany(t => t.ItensIncluidos)
            .WithOne(i => i.Treinamento)
            .HasForeignKey(i => i.TreinamentoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
