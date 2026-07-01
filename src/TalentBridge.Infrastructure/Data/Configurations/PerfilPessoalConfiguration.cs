using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class PerfilPessoalConfiguration : IEntityTypeConfiguration<PerfilPessoal>
{
    public void Configure(EntityTypeBuilder<PerfilPessoal> builder)
    {
        builder.ToTable("PerfisPessoais");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Cpf)
            .HasMaxLength(14);

        builder.HasIndex(p => p.Cpf)
            .IsUnique()
            .HasFilter("\"Cpf\" IS NOT NULL")
            .HasDatabaseName("IX_PerfisPessoais_Cpf");

        builder.Property(p => p.Rg)
            .HasMaxLength(20);

        builder.Property(p => p.SobreMim)
            .HasColumnType("text");

        builder.Property(p => p.DescricaoPcd)
            .HasColumnType("text");

        builder.Property(p => p.Instagram)
            .HasMaxLength(200);

        builder.Property(p => p.Facebook)
            .HasMaxLength(200);

        builder.Property(p => p.Linkedin)
            .HasMaxLength(200);

        // Value Object Endereco
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
