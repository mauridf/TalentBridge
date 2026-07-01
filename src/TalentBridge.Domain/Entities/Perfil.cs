namespace TalentBridge.Domain.Entities;

/// <summary>
/// Perfil de usuário do sistema (Admin, Gestor, Recrutador, Candidato)
/// </summary>
public class Perfil : BaseEntity
{
    /// <summary>
    /// Código único do perfil (ex: "ADMIN", "CANDIDATO")
    /// </summary>
    public string Codigo { get; private set; } = string.Empty;

    /// <summary>
    /// Nome descritivo do perfil
    /// </summary>
    public string Nome { get; private set; } = string.Empty;

    /// <summary>
    /// Descrição do perfil e suas permissões
    /// </summary>
    public string? Descricao { get; private set; }

    // Relacionamentos
    public ICollection<Usuario> Usuarios { get; private set; } = new List<Usuario>();
    public ICollection<Funcionalidade> Funcionalidades { get; private set; } = new List<Funcionalidade>();

    // Construtor para EF Core
    protected Perfil() { }

    public Perfil(string codigo, string nome, string? descricao = null)
    {
        Codigo = codigo.ToUpperInvariant();
        Nome = nome;
        Descricao = descricao;
    }
}
