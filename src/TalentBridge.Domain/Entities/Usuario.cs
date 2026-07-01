using TalentBridge.Domain.Enums;

namespace TalentBridge.Domain.Entities;

/// <summary>
/// Entidade base para todos os usuários do sistema (TPH - Table Per Hierarchy).
/// Subtipos: Candidato, Gestor, Recrutador.
/// </summary>
public abstract class Usuario : BaseEntity
{
    /// <summary>
    /// Nome completo do usuário
    /// </summary>
    public string Nome { get; private set; } = string.Empty;

    /// <summary>
    /// Email único do usuário (usado para login)
    /// </summary>
    public string Email { get; private set; } = string.Empty;

    /// <summary>
    /// Hash da senha (BCrypt)
    /// </summary>
    public string SenhaHash { get; private set; } = string.Empty;

    /// <summary>
    /// Telefone de contato
    /// </summary>
    public string? Telefone { get; private set; }

    /// <summary>
    /// Status atual do usuário
    /// </summary>
    public StatusUsuarioEnum Status { get; private set; } = StatusUsuarioEnum.Pendente;

    /// <summary>
    /// Origem do cadastro
    /// </summary>
    public OrigemCadastroEnum OrigemCadastro { get; private set; } = OrigemCadastroEnum.Web;

    /// <summary>
    /// Perfil do usuário no sistema
    /// </summary>
    public Guid PerfilId { get; private set; }
    public Perfil Perfil { get; private set; } = null!;

    /// <summary>
    /// Discriminador para TPH (EF Core)
    /// </summary>
    public string Discriminator { get; private set; } = string.Empty;

    /// <summary>
    /// Refresh token para renovação de sessão
    /// </summary>
    public string? RefreshToken { get; private set; }

    /// <summary>
    /// Data de expiração do refresh token
    /// </summary>
    public DateTime? RefreshTokenExpiryTime { get; private set; }

    /// <summary>
    /// Data do último login
    /// </summary>
    public DateTime? UltimoLogin { get; private set; }

    // Relacionamentos
    public ICollection<UsuarioEmpresa> UsuarioEmpresas { get; private set; } = new List<UsuarioEmpresa>();

    protected Usuario() { }

    protected Usuario(string nome, string email, string senhaHash, Guid perfilId, string discriminator)
    {
        Nome = nome;
        Email = email.ToLowerInvariant().Trim();
        SenhaHash = senhaHash;
        PerfilId = perfilId;
        Discriminator = discriminator;
        Status = StatusUsuarioEnum.Pendente;
    }

    /// <summary>
    /// Atualiza o refresh token do usuário
    /// </summary>
    public void AtualizarRefreshToken(string? refreshToken, DateTime? expiryTime)
    {
        RefreshToken = refreshToken;
        RefreshTokenExpiryTime = expiryTime;
    }

    /// <summary>
    /// Registra o último login do usuário
    /// </summary>
    public void RegistrarLogin()
    {
        UltimoLogin = DateTime.UtcNow;
        AtualizarDataModificacao();
    }

    /// <summary>
    /// Ativa o usuário (após confirmação de email)
    /// </summary>
    public void Ativar()
    {
        Status = StatusUsuarioEnum.Ativo;
        AtualizarDataModificacao();
    }

    /// <summary>
    /// Inativa o usuário
    /// </summary>
    public void Inativar()
    {
        Status = StatusUsuarioEnum.Inativo;
        AtualizarDataModificacao();
    }

    /// <summary>
    /// Atualiza dados básicos do usuário
    /// </summary>
    public void AtualizarDados(string nome, string? telefone)
    {
        Nome = nome;
        Telefone = telefone;
        AtualizarDataModificacao();
    }

    /// <summary>
    /// Atualiza a senha do usuário
    /// </summary>
    public void AtualizarSenha(string novoHash)
    {
        SenhaHash = novoHash;
        RefreshToken = null;
        RefreshTokenExpiryTime = null;
        AtualizarDataModificacao();
    }

    /// <summary>
    /// Verifica se o refresh token é válido
    /// </summary>
    public bool RefreshTokenValido()
    {
        return !string.IsNullOrWhiteSpace(RefreshToken) &&
               RefreshTokenExpiryTime.HasValue &&
               RefreshTokenExpiryTime.Value > DateTime.UtcNow;
    }
}
