using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class ConviteConfiguration : IEntityTypeConfiguration<Convite>
{
    public void Configure(EntityTypeBuilder<Convite> builder)
    {
        builder.ToTable("Convites");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Email)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(c => c.Cnpj)
            .HasMaxLength(14);

        builder.Property(c => c.NomeEmpresa)
            .HasMaxLength(200);

        builder.Property(c => c.NomeResponsavel)
            .HasMaxLength(128);

        builder.Property(c => c.Telefone)
            .HasMaxLength(20);

        builder.Property(c => c.Status)
            .IsRequired();

        builder.Property(c => c.Tipo)
            .IsRequired();

        builder.Property(c => c.Token)
            .IsRequired();

        builder.Property(c => c.DataExpiracao)
            .IsRequired();

        builder.HasOne(c => c.EmpresaResponsavel)
            .WithMany()
            .HasForeignKey(c => c.EmpresaResponsavelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(c => c.Token)
            .HasDatabaseName("IX_Convites_Token");

        builder.HasIndex(c => c.Email)
            .HasDatabaseName("IX_Convites_Email");
    }
}
