using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class EmpresaConfiguration : IEntityTypeConfiguration<Empresa>
{
    public void Configure(EntityTypeBuilder<Empresa> builder)
    {
        builder.ToTable("Empresas");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Nome)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.CNPJ)
            .IsRequired()
            .HasMaxLength(14);

        builder.HasIndex(e => e.CNPJ)
            .IsUnique()
            .HasDatabaseName("IX_Empresas_CNPJ");

        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Telefone)
            .IsRequired()
            .HasMaxLength(20);

        // Relacionamento com Segmento
        builder.HasOne(e => e.Segmento)
            .WithMany(s => s.Empresas)
            .HasForeignKey(e => e.SegmentoId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relacionamento com Parceiro
        builder.HasOne(e => e.Parceiro)
            .WithMany(p => p.Empresas)
            .HasForeignKey(e => e.ParceiroId)
            .OnDelete(DeleteBehavior.SetNull);

        // Configuração do Value Object Endereco como Owned
        builder.OwnsOne(e => e.Endereco, endereco =>
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

        // Configurações de notificação
        builder.Property(e => e.DesativarNotificacoes).HasDefaultValue(false);
        builder.Property(e => e.ReceberNotificacoesCandidaturas).HasDefaultValue(true);
        builder.Property(e => e.EnviarEmailNovoLoginNavegador).HasDefaultValue(true);
        builder.Property(e => e.EnviarEmailAtividadeIncomum).HasDefaultValue(true);

        builder.HasIndex(e => e.SegmentoId).HasDatabaseName("IX_Empresas_SegmentoId");
    }
}
