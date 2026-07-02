using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class ItemIncluidoConfiguration : IEntityTypeConfiguration<ItemIncluido>
{
    public void Configure(EntityTypeBuilder<ItemIncluido> builder)
    {
        builder.ToTable("ItensIncluidos");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Descricao)
            .HasMaxLength(500)
            .IsRequired();
    }
}
