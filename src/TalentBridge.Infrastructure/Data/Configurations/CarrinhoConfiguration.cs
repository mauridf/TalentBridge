using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class CarrinhoConfiguration : IEntityTypeConfiguration<Carrinho>
{
    public void Configure(EntityTypeBuilder<Carrinho> builder)
    {
        builder.ToTable("Carrinhos");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Status)
            .IsRequired();

        builder.HasOne(c => c.Empresa)
            .WithMany()
            .HasForeignKey(c => c.EmpresaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(c => c.EmpresaId)
            .HasDatabaseName("IX_Carrinhos_EmpresaId");
    }
}
