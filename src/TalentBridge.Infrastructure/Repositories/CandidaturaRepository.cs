using Microsoft.EntityFrameworkCore;
using TalentBridge.Domain.Entities;
using TalentBridge.Domain.Interfaces.Repositories;
using TalentBridge.Infrastructure.Data;

namespace TalentBridge.Infrastructure.Repositories;

public class CandidaturaRepository : Repository<Candidatura>, ICandidaturaRepository
{
    public CandidaturaRepository(TalentBridgeDbContext context) : base(context)
    {
    }

    public async Task<bool> ExisteCandidaturaAsync(Guid vagaId, Guid candidatoId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(c => c.VagaId == vagaId && c.CandidatoId == candidatoId, cancellationToken);
    }

    public async Task<IEnumerable<Candidatura>> GetByVagaIdWithCandidatoAsync(Guid vagaId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(c => c.Candidato)
            .Where(c => c.VagaId == vagaId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Candidatura>> GetByCandidatoIdWithVagaAsync(Guid candidatoId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(c => c.Vaga)
                .ThenInclude(v => v.Empresa)
            .Where(c => c.CandidatoId == candidatoId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
