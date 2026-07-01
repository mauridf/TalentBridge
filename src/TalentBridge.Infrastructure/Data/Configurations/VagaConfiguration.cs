using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class VagaConfiguration : IEntityTypeConfiguration<Vaga>
{
    public void Configure(EntityTypeBuilder<Vaga> builder)
    {
        builder.ToTable("Vagas");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Titulo)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(v => v.Cargo)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(v => v.Descricao)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(v => v.Atividades)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(v => v.Beneficios)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(v => v.DiferenciaisConsiderados)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(v => v.Salario)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(v => v.Status)
            .IsRequired();

        builder.Property(v => v.Encerrada)
            .HasDefaultValue(false);

        builder.Property(v => v.QuantidadeVagas)
            .HasDefaultValue(1);

        // Relacionamento com Empresa
        builder.HasOne(v => v.Empresa)
            .WithMany(e => e.Vagas)
            .HasForeignKey(v => v.EmpresaId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relacionamento com UsuarioCriacao
        builder.HasOne(v => v.UsuarioCriacao)
            .WithMany()
            .HasForeignKey(v => v.UsuarioCriacaoId)
            .OnDelete(DeleteBehavior.Restrict);

        // Índices para buscas frequentes
        builder.HasIndex(v => v.Status).HasDatabaseName("IX_Vagas_Status");
        builder.HasIndex(v => v.EmpresaId).HasDatabaseName("IX_Vagas_EmpresaId");
        builder.HasIndex(v => v.Cidade).HasDatabaseName("IX_Vagas_Cidade");
        builder.HasIndex(v => v.Estado).HasDatabaseName("IX_Vagas_Estado");
        builder.HasIndex(v => v.DataCandidaturaFim).HasDatabaseName("IX_Vagas_DataCandidaturaFim");
    }
}
