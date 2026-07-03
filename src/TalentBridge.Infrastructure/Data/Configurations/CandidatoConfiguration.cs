using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class CandidatoConfiguration : IEntityTypeConfiguration<Candidato>
{
    public void Configure(EntityTypeBuilder<Candidato> builder)
    {
        builder.ToTable("Candidatos");
        builder.Property(c => c.NomeSocial)
            .HasMaxLength(100);

        builder.Property(c => c.DataNascimento)
            .IsRequired();

        // Relacionamento 1:1 com PerfilPessoal
        builder.HasOne(c => c.PerfilPessoal)
            .WithOne(p => p.Candidato)
            .HasForeignKey<Candidato>(c => c.PerfilPessoalId)
            .OnDelete(DeleteBehavior.SetNull);

        // Relacionamento 1:1 com PerfilProfissional
        builder.HasOne(c => c.PerfilProfissional)
            .WithOne(p => p.Candidato)
            .HasForeignKey<Candidato>(c => c.PerfilProfissionalId)
            .OnDelete(DeleteBehavior.SetNull);

        // Relacionamento com Parceiro
        builder.HasOne(c => c.Parceiro)
            .WithMany(p => p.Candidatos)
            .HasForeignKey(c => c.ParceiroId)
            .OnDelete(DeleteBehavior.SetNull);

        // Índice para token de confirmação
        builder.HasIndex(c => c.TokenConfirmacaoEmail)
            .HasDatabaseName("IX_Candidatos_TokenConfirmacaoEmail");
    }
}
