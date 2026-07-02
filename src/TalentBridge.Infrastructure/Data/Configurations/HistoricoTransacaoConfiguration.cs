using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class HistoricoTransacaoConfiguration : IEntityTypeConfiguration<HistoricoTransacao>
{
    public void Configure(EntityTypeBuilder<HistoricoTransacao> builder)
    {
        builder.ToTable("HistoricoTransacoes");

        builder.HasKey(ht => ht.Id);

        builder.Property(ht => ht.CnpjEmpresa)
            .HasMaxLength(14)
            .IsRequired();

        builder.Property(ht => ht.DescricaoTransacao)
            .HasMaxLength(350)
            .IsRequired();

        builder.Property(ht => ht.PerfilResponsavel)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(ht => ht.DataTransacao)
            .IsRequired();

        builder.Property(ht => ht.ValorPago)
            .HasColumnType("decimal(18,2)");

        builder.HasOne(ht => ht.Empresa)
            .WithMany()
            .HasForeignKey(ht => ht.EmpresaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ht => ht.Vaga)
            .WithMany()
            .HasForeignKey(ht => ht.VagaId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(ht => ht.CreditoEmpresa)
            .WithMany(ce => ce.HistoricoTransacoes)
            .HasForeignKey(ht => ht.CreditoEmpresaId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(ht => ht.CreditoVaga)
            .WithMany(cv => cv.HistoricoTransacoes)
            .HasForeignKey(ht => ht.CreditoVagaId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(ht => ht.EmpresaId)
            .HasDatabaseName("IX_HistoricoTransacoes_EmpresaId");

        builder.HasIndex(ht => ht.VagaId)
            .HasDatabaseName("IX_HistoricoTransacoes_VagaId");

        builder.HasIndex(ht => ht.CreditoEmpresaId)
            .HasDatabaseName("IX_HistoricoTransacoes_CreditoEmpresaId");
    }
}
