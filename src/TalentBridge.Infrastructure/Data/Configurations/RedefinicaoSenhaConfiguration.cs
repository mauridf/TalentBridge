using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class RedefinicaoSenhaConfiguration : IEntityTypeConfiguration<RedefinicaoSenha>
{
    public void Configure(EntityTypeBuilder<RedefinicaoSenha> builder)
    {
        builder.ToTable("RedefinicoesSenha");

        builder.HasKey(rs => rs.Id);

        builder.Property(rs => rs.Token)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(rs => rs.DataExpiracao)
            .IsRequired();

        builder.HasOne(rs => rs.Usuario)
            .WithMany()
            .HasForeignKey(rs => rs.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(rs => rs.UsuarioId)
            .HasDatabaseName("IX_RedefinicoesSenha_UsuarioId");
    }
}
