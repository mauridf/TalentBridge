using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Nome)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("IX_Usuarios_Email");

        builder.Property(u => u.SenhaHash)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(u => u.Telefone)
            .HasMaxLength(20);

        builder.Property(u => u.Status)
            .IsRequired();

        builder.Property(u => u.OrigemCadastro)
            .IsRequired();

        builder.Property(u => u.RefreshToken)
            .HasMaxLength(500);

        // Relacionamento com Perfil
        builder.HasOne(u => u.Perfil)
            .WithMany(p => p.Usuarios)
            .HasForeignKey(u => u.PerfilId)
            .OnDelete(DeleteBehavior.Restrict);

        // Índices
        builder.HasIndex(u => u.PerfilId)
            .HasDatabaseName("IX_Usuarios_PerfilId");

        builder.HasIndex(u => u.RefreshToken)
            .HasDatabaseName("IX_Usuarios_RefreshToken");
    }
}
