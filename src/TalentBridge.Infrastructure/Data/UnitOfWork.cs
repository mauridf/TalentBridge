using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TalentBridge.Domain.Interfaces;
using TalentBridge.Domain.Interfaces.Repositories;
using TalentBridge.Infrastructure.Repositories;

namespace TalentBridge.Infrastructure.Data;

/// <summary>
/// Implementação do padrão Unit of Work.
/// Gerencia transações e centraliza o acesso aos repositórios.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly TalentBridgeDbContext _context;
    private IDbContextTransaction? _currentTransaction;

    // Repositórios (Lazy Loading)
    private IUsuarioRepository? _usuarios;
    private ICandidatoRepository? _candidatos;
    private IEmpresaRepository? _empresas;
    private IVagaRepository? _vagas;
    private ICandidaturaRepository? _candidaturas;
    private IRepository<Domain.Entities.Perfil>? _perfils;
    private IRepository<Domain.Entities.Competencia>? _competencias;
    private IRepository<Domain.Entities.Dominio>? _dominios;
    private IRepository<Domain.Entities.Segmento>? _segmentos;
    private IRepository<Domain.Entities.Parceiro>? _parceiros;
    private IRepository<Domain.Entities.Cupom>? _cupons;
    private IRepository<Domain.Entities.Convite>? _convites;
    private IRepository<Domain.Entities.Produto>? _produtos;
    private IRepository<Domain.Entities.Carrinho>? _carrinhos;
    private IRepository<Domain.Entities.Pedido>? _pedidos;
    private IRepository<Domain.Entities.PerfilPessoal>? _perfisPessoais;
    private IRepository<Domain.Entities.PerfilProfissional>? _perfisProfissionais;

    public UnitOfWork(TalentBridgeDbContext context)
    {
        _context = context;
    }

    public IUsuarioRepository Usuarios =>
        _usuarios ??= new UsuarioRepository(_context);

    public ICandidatoRepository Candidatos =>
        _candidatos ??= new CandidatoRepository(_context);

    public IEmpresaRepository Empresas =>
        _empresas ??= new EmpresaRepository(_context);

    public IVagaRepository Vagas =>
        _vagas ??= new VagaRepository(_context);

    public ICandidaturaRepository Candidaturas =>
        _candidaturas ??= new CandidaturaRepository(_context);

    public IRepository<Domain.Entities.Perfil> Perfils =>
        _perfils ??= new Repository<Domain.Entities.Perfil>(_context);

    public IRepository<Domain.Entities.Competencia> Competencias =>
        _competencias ??= new Repository<Domain.Entities.Competencia>(_context);

    public IRepository<Domain.Entities.Dominio> Dominios =>
        _dominios ??= new Repository<Domain.Entities.Dominio>(_context);

    public IRepository<Domain.Entities.Segmento> Segmentos =>
        _segmentos ??= new Repository<Domain.Entities.Segmento>(_context);

    public IRepository<Domain.Entities.Parceiro> Parceiros =>
        _parceiros ??= new Repository<Domain.Entities.Parceiro>(_context);

    public IRepository<Domain.Entities.Cupom> Cupons =>
        _cupons ??= new Repository<Domain.Entities.Cupom>(_context);

    public IRepository<Domain.Entities.Convite> Convites =>
        _convites ??= new Repository<Domain.Entities.Convite>(_context);

    public IRepository<Domain.Entities.Produto> Produtos =>
        _produtos ??= new Repository<Domain.Entities.Produto>(_context);

    public IRepository<Domain.Entities.Carrinho> Carrinhos =>
        _carrinhos ??= new Repository<Domain.Entities.Carrinho>(_context);

    public IRepository<Domain.Entities.Pedido> Pedidos =>
        _pedidos ??= new Repository<Domain.Entities.Pedido>(_context);

    public IRepository<Domain.Entities.PerfilPessoal> PerfisPessoais =>
        _perfisPessoais ??= new Repository<Domain.Entities.PerfilPessoal>(_context);

    public IRepository<Domain.Entities.PerfilProfissional> PerfisProfissionais =>
        _perfisProfissionais ??= new Repository<Domain.Entities.PerfilProfissional>(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _currentTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction == null)
            throw new InvalidOperationException("Nenhuma transação ativa.");

        await _currentTransaction.CommitAsync(cancellationToken);
        await _currentTransaction.DisposeAsync();
        _currentTransaction = null;
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction == null)
            throw new InvalidOperationException("Nenhuma transação ativa.");

        await _currentTransaction.RollbackAsync(cancellationToken);
        await _currentTransaction.DisposeAsync();
        _currentTransaction = null;
    }

    public void Dispose()
    {
        _currentTransaction?.Dispose();
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
