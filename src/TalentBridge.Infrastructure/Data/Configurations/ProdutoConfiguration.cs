using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        builder.ToTable("Produtos");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.NomeProduto)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(p => p.DescricaoProduto)
            .HasMaxLength(500);

        builder.Property(p => p.ValorProduto)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(p => p.QuantidadeCreditoPorVaga)
            .HasDefaultValue(1);
    }
}
