using Microsoft.EntityFrameworkCore;
using TalentBridge.Domain.Entities;
using TalentBridge.Infrastructure.Data.Configurations;

namespace TalentBridge.Infrastructure.Data;

/// <summary>
/// Contexto principal do Entity Framework Core para o TalentBridge.
/// Gerencia todas as entidades, configurações e relacionamentos.
/// </summary>
public class TalentBridgeDbContext : DbContext
{
    public TalentBridgeDbContext(DbContextOptions<TalentBridgeDbContext> options)
        : base(options)
    {
    }

    // ==========================================
    // DbSets - Tabelas do banco de dados
    // ==========================================

    // Usuários (TPT)
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Admin> Admins => Set<Admin>();
    public DbSet<Candidato> Candidatos => Set<Candidato>();
    public DbSet<Gestor> Gestores => Set<Gestor>();
    public DbSet<Recrutador> Recrutadores => Set<Recrutador>();

    // Perfil e Autorização
    public DbSet<Perfil> Perfils => Set<Perfil>();
    public DbSet<Funcionalidade> Funcionalidades => Set<Funcionalidade>();
    public DbSet<Operacao> Operacoes => Set<Operacao>();

    // Empresa
    public DbSet<Empresa> Empresas => Set<Empresa>();
    public DbSet<Segmento> Segmentos => Set<Segmento>();

    // Vagas e Competências
    public DbSet<Vaga> Vagas => Set<Vaga>();
    public DbSet<Competencia> Competencias => Set<Competencia>();
    public DbSet<CompetenciaVaga> CompetenciasVagas => Set<CompetenciaVaga>();
    public DbSet<CompetenciaCandidato> CompetenciasCandidatos => Set<CompetenciaCandidato>();
    public DbSet<CompetenciaTreinamento> CompetenciasTreinamentos => Set<CompetenciaTreinamento>();

    // Candidaturas
    public DbSet<Candidatura> Candidaturas => Set<Candidatura>();
    public DbSet<Visita> Visitas => Set<Visita>();

    // Perfil do Candidato
    public DbSet<PerfilPessoal> PerfisPessoais => Set<PerfilPessoal>();
    public DbSet<PerfilProfissional> PerfisProfissionais => Set<PerfilProfissional>();
    public DbSet<FormacaoAcademica> FormacoesAcademicas => Set<FormacaoAcademica>();
    public DbSet<ExperienciaProfissional> ExperienciasProfissionais => Set<ExperienciaProfissional>();
    public DbSet<Curso> Cursos => Set<Curso>();
    public DbSet<AreaInteresse> AreasInteresse => Set<AreaInteresse>();

    // Parceiros e Cupons
    public DbSet<Parceiro> Parceiros => Set<Parceiro>();
    public DbSet<Cupom> Cupons => Set<Cupom>();
    public DbSet<Convite> Convites => Set<Convite>();
    public DbSet<UsuarioEmpresa> UsuariosEmpresas => Set<UsuarioEmpresa>();

    // Créditos e Pagamentos
    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<CreditosEmpresa> CreditosEmpresas => Set<CreditosEmpresa>();
    public DbSet<CreditoVagas> CreditoVagas => Set<CreditoVagas>();
    public DbSet<Carrinho> Carrinhos => Set<Carrinho>();
    public DbSet<ItemCarrinho> ItensCarrinho => Set<ItemCarrinho>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<ItemPedido> ItensPedido => Set<ItemPedido>();
    public DbSet<HistoricoTransacao> HistoricoTransacoes => Set<HistoricoTransacao>();

    // Domínio e Treinamento
    public DbSet<Dominio> Dominios => Set<Dominio>();
    public DbSet<Treinamento> Treinamentos => Set<Treinamento>();

    // Contatos
    public DbSet<Contato> Contatos => Set<Contato>();

    // Treinamento com módulos e conteúdos
    public DbSet<ModuloCurso> ModulosCursos => Set<ModuloCurso>();
    public DbSet<ConteudoModulo> ConteudosModulos => Set<ConteudoModulo>();
    public DbSet<ItemIncluido> ItensIncluidos => Set<ItemIncluido>();

    // Configurações e logs
    public DbSet<ParametrosGerais> ParametrosGerais => Set<ParametrosGerais>();
    public DbSet<ApiLog> ApiLogs => Set<ApiLog>();

    // Outros
    public DbSet<RedefinicaoSenha> RedefinicoesSenha => Set<RedefinicaoSenha>();

    /// <summary>
    /// Configura o modelo de dados usando Fluent API
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplicar todas as configurações do assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TalentBridgeDbContext).Assembly);
    }

    /// <summary>
    /// Hook para auditar CreatedAt/UpdatedAt automaticamente
    /// </summary>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AtualizarDatasAuditoria();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void AtualizarDatasAuditoria()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity &&
                        (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (BaseEntity)entry.Entity;

            if (entry.State == EntityState.Added)
            {
                // CreatedAt e UpdatedAt já são definidos no construtor da BaseEntity
            }
            else if (entry.State == EntityState.Modified)
            {
                entity.AtualizarDataModificacao();
            }
        }
    }
}
