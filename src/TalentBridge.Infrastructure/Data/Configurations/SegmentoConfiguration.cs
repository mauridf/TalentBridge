using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class SegmentoConfiguration : IEntityTypeConfiguration<Segmento>
{
    public void Configure(EntityTypeBuilder<Segmento> builder)
    {
        builder.ToTable("Segmentos");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Nome)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(s => s.Descricao)
            .HasMaxLength(500);
    }
}
