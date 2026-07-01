using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
{
    public void Configure(EntityTypeBuilder<Pedido> builder)
    {
        builder.ToTable("Pedidos");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.NumeroPedido)
            .IsRequired();

        builder.HasIndex(p => p.NumeroPedido)
            .IsUnique()
            .HasDatabaseName("IX_Pedidos_NumeroPedido");

        builder.Property(p => p.IdCheckout)
            .HasMaxLength(200);

        builder.Property(p => p.Status)
            .IsRequired();

        // Relacionamento com Carrinho (1:1)
        builder.HasOne(p => p.Carrinho)
            .WithMany()
            .HasForeignKey(p => p.CarrinhoId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relacionamento com Empresa
        builder.HasOne(p => p.Empresa)
            .WithMany()
            .HasForeignKey(p => p.EmpresaId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relacionamento com Cupom (opcional)
        builder.HasOne(p => p.Cupom)
            .WithMany(c => c.Pedidos)
            .HasForeignKey(p => p.CupomId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
