using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class CreditoVagasConfiguration : IEntityTypeConfiguration<CreditoVagas>
{
    public void Configure(EntityTypeBuilder<CreditoVagas> builder)
    {
        builder.ToTable("CreditoVagas");

        builder.HasKey(cv => cv.Id);

        builder.Property(cv => cv.QuantidadeLiberada)
            .IsRequired();

        builder.HasOne(cv => cv.Empresa)
            .WithMany()
            .HasForeignKey(cv => cv.EmpresaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(cv => cv.Vaga)
            .WithMany()
            .HasForeignKey(cv => cv.VagaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(cv => cv.Produto)
            .WithMany()
            .HasForeignKey(cv => cv.ProdutoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(cv => cv.CreditoEmpresa)
            .WithMany(ce => ce.CreditoVagas)
            .HasForeignKey(cv => cv.CreditoEmpresaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(cv => cv.EmpresaId)
            .HasDatabaseName("IX_CreditoVagas_EmpresaId");

        builder.HasIndex(cv => cv.VagaId)
            .HasDatabaseName("IX_CreditoVagas_VagaId");

        builder.HasIndex(cv => cv.CreditoEmpresaId)
            .HasDatabaseName("IX_CreditoVagas_CreditoEmpresaId");
    }
}
