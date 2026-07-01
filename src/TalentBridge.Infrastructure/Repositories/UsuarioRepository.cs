using Microsoft.EntityFrameworkCore;
using TalentBridge.Domain.Entities;
using TalentBridge.Domain.Interfaces.Repositories;
using TalentBridge.Infrastructure.Data;

namespace TalentBridge.Infrastructure.Repositories;

public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(TalentBridgeDbContext context) : base(context)
    {
    }

    public async Task<Usuario?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant().Trim(), cancellationToken);
    }

    public async Task<Usuario?> GetByIdWithPerfilAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(u => u.Perfil)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<Usuario?> GetByEmailWithRefreshTokenAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant().Trim()
                                    && u.RefreshToken != null, cancellationToken);
    }

    public async Task<Usuario?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken, cancellationToken);
    }

    public async Task<bool> EmailExisteAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(u => u.Email == email.ToLowerInvariant().Trim(), cancellationToken);
    }

    public async Task<Usuario?> GetByEmailWithPerfisAndEmpresasAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(u => u.Perfil)
            .Include(u => u.UsuarioEmpresas)
                .ThenInclude(ue => ue.Empresa)
            .FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant().Trim(), cancellationToken);
    }
}
