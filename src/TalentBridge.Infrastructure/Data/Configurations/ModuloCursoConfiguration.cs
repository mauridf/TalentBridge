using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class ModuloCursoConfiguration : IEntityTypeConfiguration<ModuloCurso>
{
    public void Configure(EntityTypeBuilder<ModuloCurso> builder)
    {
        builder.ToTable("ModulosCursos");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Nome)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(m => m.Descricao)
            .HasMaxLength(1000);

        // 1:N com ConteudoModulo
        builder.HasMany(m => m.Conteudos)
            .WithOne(c => c.ModuloCurso)
            .HasForeignKey(c => c.ModuloCursoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
