using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class ConteudoModuloConfiguration : IEntityTypeConfiguration<ConteudoModulo>
{
    public void Configure(EntityTypeBuilder<ConteudoModulo> builder)
    {
        builder.ToTable("ConteudosModulos");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Titulo)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(c => c.Descricao)
            .HasMaxLength(2000);

        builder.Property(c => c.UrlVideo)
            .HasMaxLength(500);

        builder.Property(c => c.UrlMaterial)
            .HasMaxLength(500);
    }
}
