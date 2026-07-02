using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class ItemCarrinhoConfiguration : IEntityTypeConfiguration<ItemCarrinho>
{
    public void Configure(EntityTypeBuilder<ItemCarrinho> builder)
    {
        builder.ToTable("ItensCarrinho");

        builder.HasKey(ic => ic.Id);

        builder.Property(ic => ic.Quantidade)
            .IsRequired();

        builder.Property(ic => ic.ValorUnitario)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.HasOne(ic => ic.Carrinho)
            .WithMany(c => c.ItensCarrinho)
            .HasForeignKey(ic => ic.CarrinhoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ic => ic.Produto)
            .WithMany(p => p.ItensCarrinho)
            .HasForeignKey(ic => ic.ProdutoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(ic => ic.CarrinhoId)
            .HasDatabaseName("IX_ItensCarrinho_CarrinhoId");

        builder.HasIndex(ic => ic.ProdutoId)
            .HasDatabaseName("IX_ItensCarrinho_ProdutoId");
    }
}
