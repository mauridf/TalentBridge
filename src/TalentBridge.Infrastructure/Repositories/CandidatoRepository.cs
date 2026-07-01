using Microsoft.EntityFrameworkCore;
using TalentBridge.Domain.Entities;
using TalentBridge.Domain.Interfaces.Repositories;
using TalentBridge.Infrastructure.Data;

namespace TalentBridge.Infrastructure.Repositories;

public class CandidatoRepository : Repository<Candidato>, ICandidatoRepository
{
    public CandidatoRepository(TalentBridgeDbContext context) : base(context)
    {
    }

    public async Task<Candidato?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.Email == email.ToLowerInvariant().Trim(), cancellationToken);
    }

    public async Task<Candidato?> GetByIdCompletoAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.PerfilPessoal)
            .Include(c => c.PerfilProfissional)
                .ThenInclude(pp => pp!.FormacoesAcademicas)
            .Include(c => c.PerfilProfissional)
                .ThenInclude(pp => pp!.ExperienciasProfissionais)
            .Include(c => c.PerfilProfissional)
                .ThenInclude(pp => pp!.CompetenciasCandidatos)
                    .ThenInclude(cc => cc.Competencia)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Candidato?> GetByTokenConfirmacaoAsync(Guid token, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.TokenConfirmacaoEmail == token, cancellationToken);
    }

    public async Task<IEnumerable<Candidato>> GetByParceiroIdAsync(Guid parceiroId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(c => c.ParceiroId == parceiroId)
            .ToListAsync(cancellationToken);
    }
}
