namespace TalentBridge.Domain.Entities;

/// <summary>
/// Funcionalidade do sistema (ex: ADMIN_DADOS, EMPRESA_DADOS)
/// </summary>
public class Funcionalidade : BaseEntity
{
    public string Codigo { get; private set; } = string.Empty;
    public string Descricao { get; private set; } = string.Empty;

    // Relacionamentos
    public ICollection<Operacao> Operacoes { get; private set; } = new List<Operacao>();
    public ICollection<Perfil> Perfis { get; private set; } = new List<Perfil>();

    protected Funcionalidade() { }

    public Funcionalidade(string codigo, string descricao)
    {
        Codigo = codigo.ToUpperInvariant();
        Descricao = descricao;
    }
}
