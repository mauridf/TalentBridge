using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class CreditosEmpresaConfiguration : IEntityTypeConfiguration<CreditosEmpresa>
{
    public void Configure(EntityTypeBuilder<CreditosEmpresa> builder)
    {
        builder.ToTable("CreditosEmpresas");

        builder.HasKey(ce => ce.Id);

        builder.Property(ce => ce.Creditos)
            .IsRequired();

        builder.HasOne(ce => ce.Empresa)
            .WithMany(e => e.CreditosEmpresa)
            .HasForeignKey(ce => ce.EmpresaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ce => ce.Produto)
            .WithMany(p => p.CreditosEmpresas)
            .HasForeignKey(ce => ce.ProdutoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(ce => new { ce.EmpresaId, ce.ProdutoId })
            .IsUnique()
            .HasDatabaseName("IX_CreditosEmpresas_EmpresaId_ProdutoId");
    }
}
