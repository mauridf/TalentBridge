using Microsoft.EntityFrameworkCore;
using TalentBridge.Domain.Entities;
using TalentBridge.Domain.Interfaces.Repositories;
using TalentBridge.Infrastructure.Data;

namespace TalentBridge.Infrastructure.Repositories;

public class EmpresaRepository : Repository<Empresa>, IEmpresaRepository
{
    public EmpresaRepository(TalentBridgeDbContext context) : base(context)
    {
    }

    public async Task<Empresa?> GetByCnpjAsync(string cnpj, CancellationToken cancellationToken = default)
    {
        var cnpjLimpo = cnpj?.Replace(".", "").Replace("-", "").Replace("/", "") ?? string.Empty;
        return await _dbSet
            .FirstOrDefaultAsync(e => e.CNPJ == cnpjLimpo, cancellationToken);
    }

    public async Task<IEnumerable<Empresa>> GetByUsuarioIdAsync(Guid usuarioId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<UsuarioEmpresa>()
            .Where(ue => ue.UsuarioId == usuarioId)
            .Select(ue => ue.Empresa)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Empresa>> GetByParceiroIdAsync(Guid parceiroId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(e => e.ParceiroId == parceiroId)
            .ToListAsync(cancellationToken);
    }
}
