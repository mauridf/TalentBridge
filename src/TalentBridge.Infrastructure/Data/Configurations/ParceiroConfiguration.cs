using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class ParceiroConfiguration : IEntityTypeConfiguration<Parceiro>
{
    public void Configure(EntityTypeBuilder<Parceiro> builder)
    {
        builder.ToTable("Parceiros");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Nome)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.NomeSocial)
            .HasMaxLength(100);

        builder.Property(p => p.Email)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasIndex(p => p.Email)
            .IsUnique()
            .HasDatabaseName("IX_Parceiros_Email");

        builder.Property(p => p.Telefone)
            .HasMaxLength(20);

        builder.Property(p => p.TipoPessoa)
            .HasMaxLength(2)
            .IsRequired();

        builder.Property(p => p.Documento)
            .HasMaxLength(14)
            .IsRequired();

        builder.HasIndex(p => p.Documento)
            .IsUnique()
            .HasDatabaseName("IX_Parceiros_Documento");

        builder.Property(p => p.CodigoPublico)
            .HasMaxLength(20)
            .IsRequired();

        builder.HasIndex(p => p.CodigoPublico)
            .IsUnique()
            .HasDatabaseName("IX_Parceiros_CodigoPublico");

        builder.Property(p => p.Origem)
            .HasMaxLength(100);

        builder.Property(p => p.Status)
            .IsRequired();

        builder.Property(p => p.WalletId)
            .HasMaxLength(200);

        builder.Property(p => p.PercentualSplit)
            .HasColumnType("decimal(5,2)");

        builder.Property(p => p.RendaMensal)
            .HasColumnType("decimal(18,2)");

        builder.OwnsOne(p => p.Endereco, endereco =>
        {
            endereco.Property(e => e.CEP).HasColumnName("CEP").HasMaxLength(8);
            endereco.Property(e => e.Rua).HasColumnName("Rua").HasMaxLength(200);
            endereco.Property(e => e.Numero).HasColumnName("Numero").HasMaxLength(10);
            endereco.Property(e => e.Bairro).HasColumnName("Bairro").HasMaxLength(100);
            endereco.Property(e => e.Cidade).HasColumnName("Cidade").HasMaxLength(100);
            endereco.Property(e => e.Estado).HasColumnName("Estado").HasMaxLength(2);
            endereco.Property(e => e.Complemento).HasColumnName("Complemento").HasMaxLength(200);
            endereco.Property(e => e.Latitude).HasColumnName("Latitude");
            endereco.Property(e => e.Longitude).HasColumnName("Longitude");
        });
    }
}
