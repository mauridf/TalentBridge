namespace TalentBridge.Domain.Entities;

public class ParametrosGerais : BaseEntity
{
    public string Chave { get; private set; } = string.Empty;
    public string Valor { get; private set; } = string.Empty;
    public string? Descricao { get; private set; }

    protected ParametrosGerais() { }

    public ParametrosGerais(string chave, string valor, string? descricao = null)
    {
        Chave = chave.ToUpperInvariant().Trim();
        Valor = valor;
        Descricao = descricao;
    }

    public void AtualizarValor(string valor)
    {
        Valor = valor;
        AtualizarDataModificacao();
    }
}
