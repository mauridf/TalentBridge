namespace TalentBridge.Domain.Entities;

/// <summary>
/// Segmento de mercado da empresa
/// </summary>
public class Segmento : BaseEntity
{
    public string Nome { get; private set; } = string.Empty;
    public string? Descricao { get; private set; }

    // Relacionamentos
    public ICollection<Empresa> Empresas { get; private set; } = new List<Empresa>();

    protected Segmento() { }

    public Segmento(string nome, string? descricao = null)
    {
        Nome = nome;
        Descricao = descricao;
    }
}
