using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class ItemPedidoConfiguration : IEntityTypeConfiguration<ItemPedido>
{
    public void Configure(EntityTypeBuilder<ItemPedido> builder)
    {
        builder.ToTable("ItensPedido");

        builder.HasKey(ip => ip.Id);

        builder.Property(ip => ip.Quantidade)
            .IsRequired();

        builder.Property(ip => ip.ValorUnitario)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.HasOne(ip => ip.Pedido)
            .WithMany()
            .HasForeignKey(ip => ip.PedidoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ip => ip.Produto)
            .WithMany(p => p.ItensPedido)
            .HasForeignKey(ip => ip.ProdutoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(ip => ip.PedidoId)
            .HasDatabaseName("IX_ItensPedido_PedidoId");

        builder.HasIndex(ip => ip.ProdutoId)
            .HasDatabaseName("IX_ItensPedido_ProdutoId");
    }
}
