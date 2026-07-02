using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class OperacaoConfiguration : IEntityTypeConfiguration<Operacao>
{
    public void Configure(EntityTypeBuilder<Operacao> builder)
    {
        builder.ToTable("Operacoes");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Codigo)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(o => o.Codigo)
            .IsUnique()
            .HasDatabaseName("IX_Operacoes_Codigo");

        builder.Property(o => o.Descricao)
            .HasMaxLength(500)
            .IsRequired();

        builder.HasOne(o => o.Funcionalidade)
            .WithMany(f => f.Operacoes)
            .HasForeignKey(o => o.FuncionalidadeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(o => o.FuncionalidadeId)
            .HasDatabaseName("IX_Operacoes_FuncionalidadeId");
    }
}
