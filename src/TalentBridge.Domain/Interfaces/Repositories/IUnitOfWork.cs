using TalentBridge.Domain.Entities;
using TalentBridge.Domain.Interfaces.Repositories;

namespace TalentBridge.Domain.Interfaces;

/// <summary>
/// Padrão Unit of Work para gerenciar transações e salvar mudanças
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IUsuarioRepository Usuarios { get; }
    ICandidatoRepository Candidatos { get; }
    IEmpresaRepository Empresas { get; }
    IVagaRepository Vagas { get; }
    ICandidaturaRepository Candidaturas { get; }
    IRepository<Perfil> Perfils { get; }
    IRepository<Competencia> Competencias { get; }
    IRepository<Dominio> Dominios { get; }
    IRepository<Segmento> Segmentos { get; }
    IRepository<Parceiro> Parceiros { get; }
    IRepository<Cupom> Cupons { get; }
    IRepository<Convite> Convites { get; }
    IRepository<Produto> Produtos { get; }
    IRepository<Carrinho> Carrinhos { get; }
    IRepository<Pedido> Pedidos { get; }
    IRepository<PerfilPessoal> PerfisPessoais { get; }
    IRepository<PerfilProfissional> PerfisProfissionais { get; }
    IRepository<Recrutador> Recrutadores { get; }

    /// <summary>
    /// Salva todas as mudanças no banco de dados
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Inicia uma transação no banco de dados
    /// </summary>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Confirma a transação atual
    /// </summary>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Desfaz a transação atual
    /// </summary>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
