using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class UsuarioEmpresaConfiguration : IEntityTypeConfiguration<UsuarioEmpresa>
{
    public void Configure(EntityTypeBuilder<UsuarioEmpresa> builder)
    {
        builder.ToTable("UsuariosEmpresas");

        builder.HasKey(ue => ue.Id);

        builder.HasOne(ue => ue.Usuario)
            .WithMany()
            .HasForeignKey(ue => ue.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ue => ue.Empresa)
            .WithMany()
            .HasForeignKey(ue => ue.EmpresaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ue => ue.Perfil)
            .WithMany()
            .HasForeignKey(ue => ue.PerfilId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(ue => new { ue.UsuarioId, ue.EmpresaId })
            .IsUnique()
            .HasDatabaseName("IX_UsuariosEmpresas_UsuarioId_EmpresaId");

        builder.HasIndex(ue => ue.PerfilId)
            .HasDatabaseName("IX_UsuariosEmpresas_PerfilId");
    }
}
