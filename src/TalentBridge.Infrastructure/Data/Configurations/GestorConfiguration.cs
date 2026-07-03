using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class GestorConfiguration : IEntityTypeConfiguration<Gestor>
{
    public void Configure(EntityTypeBuilder<Gestor> builder)
    {
        builder.ToTable("Gestores");

        builder.HasOne(g => g.Empresa)
            .WithMany(e => e.Gestores)
            .HasForeignKey(g => g.EmpresaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
