namespace TalentBridge.Domain.Entities;

/// <summary>
/// Área de interesse do candidato
/// </summary>
public class AreaInteresse : BaseEntity
{
    /// <summary>
    /// Código único da área
    /// </summary>
    public int Codigo { get; private set; }

    /// <summary>
    /// Nome da área
    /// </summary>
    public string Nome { get; private set; } = string.Empty;

    // Relacionamento N:N
    public ICollection<PerfilProfissional> PerfisProfissionais { get; private set; } = new List<PerfilProfissional>();

    protected AreaInteresse() { }

    public AreaInteresse(int codigo, string nome)
    {
        Codigo = codigo;
        Nome = nome;
    }
}
