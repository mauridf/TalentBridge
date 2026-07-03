using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class RecrutadorConfiguration : IEntityTypeConfiguration<Recrutador>
{
    public void Configure(EntityTypeBuilder<Recrutador> builder)
    {
        builder.ToTable("Recrutadores");

        builder.Property(r => r.NomeSocial)
            .HasMaxLength(100);

        builder.HasOne(r => r.Empresa)
            .WithMany(e => e.Recrutadores)
            .HasForeignKey(r => r.EmpresaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Convite)
            .WithOne(c => c.Recrutador)
            .HasForeignKey<Recrutador>(r => r.ConviteId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
