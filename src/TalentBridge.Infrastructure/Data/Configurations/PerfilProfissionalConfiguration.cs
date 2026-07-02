using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Data.Configurations;

public class PerfilProfissionalConfiguration : IEntityTypeConfiguration<PerfilProfissional>
{
    public void Configure(EntityTypeBuilder<PerfilProfissional> builder)
    {
        builder.ToTable("PerfisProfissionais");

        builder.HasKey(pp => pp.Id);
    }
}
