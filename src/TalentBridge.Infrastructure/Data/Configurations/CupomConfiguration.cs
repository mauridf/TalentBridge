using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class CupomConfiguration : IEntityTypeConfiguration<Cupom>
{
    public void Configure(EntityTypeBuilder<Cupom> builder)
    {
        builder.ToTable("Cupons");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nome)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(c => c.PercentualDesconto)
            .IsRequired();

        builder.Property(c => c.DataValidade)
            .IsRequired();

        builder.Property(c => c.Status)
            .IsRequired();

        builder.HasOne(c => c.Parceiro)
            .WithMany(p => p.Cupons)
            .HasForeignKey(c => c.ParceiroId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(c => c.ParceiroId)
            .HasDatabaseName("IX_Cupons_ParceiroId");
    }
}
