using Microsoft.EntityFrameworkCore;
using TalentBridge.Domain.Entities;
using TalentBridge.Domain.Enums;
using TalentBridge.Domain.Interfaces.Repositories;
using TalentBridge.Infrastructure.Data;

namespace TalentBridge.Infrastructure.Repositories;

public class VagaRepository : Repository<Vaga>, IVagaRepository
{
    public VagaRepository(TalentBridgeDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Vaga>> BuscarVagasAtivasAsync(
        string? termo = null,
        string? cidade = null,
        string? estado = null,
        int? areaAtuacao = null,
        int? regimeTrabalho = null,
        int? tipoContratacao = null,
        int pagina = 1,
        int tamanhoPagina = 10,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .AsNoTracking()
            .Include(v => v.Empresa)
            .Where(v => v.Status == StatusVagaEnum.Aberta
                     && !v.Encerrada
                     && v.DataCandidaturaInicio <= DateTime.UtcNow
                     && v.DataCandidaturaFim >= DateTime.UtcNow);

        if (!string.IsNullOrWhiteSpace(termo))
            query = query.Where(v => v.Titulo.Contains(termo) || v.Cargo.Contains(termo) || v.Descricao.Contains(termo));

        if (!string.IsNullOrWhiteSpace(cidade))
            query = query.Where(v => v.Cidade.Contains(cidade));

        if (!string.IsNullOrWhiteSpace(estado))
            query = query.Where(v => v.Estado == estado.ToUpperInvariant());

        if (areaAtuacao.HasValue)
            query = query.Where(v => v.AreaAtuacao == areaAtuacao.Value);

        if (regimeTrabalho.HasValue)
            query = query.Where(v => v.RegimeTrabalho == regimeTrabalho.Value);

        if (tipoContratacao.HasValue)
            query = query.Where(v => v.TipoContratacao == tipoContratacao.Value);

        return await query
            .OrderByDescending(v => v.CreatedAt)
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Vaga>> GetByEmpresaIdAsync(Guid empresaId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(v => v.EmpresaId == empresaId)
            .OrderByDescending(v => v.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Vaga>> GetVagasRecomendadasAsync(Guid candidatoId, CancellationToken cancellationToken = default)
    {
        // TODO: Implementar algoritmo de recomendação baseado no perfil do candidato
        // Por enquanto, retorna vagas abertas recentes
        return await _dbSet
            .AsNoTracking()
            .Include(v => v.Empresa)
            .Where(v => v.Status == StatusVagaEnum.Aberta && !v.Encerrada)
            .OrderByDescending(v => v.CreatedAt)
            .Take(10)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Vaga>> GetProximasVencerAsync(Guid empresaId, int diasLimite = 7, CancellationToken cancellationToken = default)
    {
        var dataLimite = DateTime.UtcNow.AddDays(diasLimite);

        return await _dbSet
            .AsNoTracking()
            .Where(v => v.EmpresaId == empresaId
                     && v.Status == StatusVagaEnum.Aberta
                     && !v.Encerrada
                     && v.DataCandidaturaFim <= dataLimite
                     && v.DataCandidaturaFim >= DateTime.UtcNow)
            .OrderBy(v => v.DataCandidaturaFim)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Vaga>> GetSimilaresAsync(Guid vagaId, int limite = 5, CancellationToken cancellationToken = default)
    {
        var vaga = await _dbSet.FindAsync(new object[] { vagaId }, cancellationToken);
        if (vaga == null) return Enumerable.Empty<Vaga>();

        return await _dbSet
            .AsNoTracking()
            .Where(v => v.Id != vagaId
                     && v.Status == StatusVagaEnum.Aberta
                     && (v.AreaAtuacao == vaga.AreaAtuacao || v.Cargo.Contains(vaga.Cargo)))
            .OrderByDescending(v => v.CreatedAt)
            .Take(limite)
            .ToListAsync(cancellationToken);
    }
}
