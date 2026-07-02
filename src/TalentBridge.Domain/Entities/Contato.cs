namespace TalentBridge.Domain.Entities;

public class Contato : BaseEntity
{
    public Guid? UsuarioId { get; private set; }
    public Usuario? Usuario { get; private set; }

    public Guid? EmpresaId { get; private set; }
    public Empresa? Empresa { get; private set; }

    public string Nome { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string? Telefone { get; private set; }
    public string? Mensagem { get; private set; }

    protected Contato() { }

    public Contato(string nome, string email, Guid? usuarioId = null, Guid? empresaId = null, string? telefone = null, string? mensagem = null)
    {
        Nome = nome;
        Email = email.ToLowerInvariant().Trim();
        UsuarioId = usuarioId;
        EmpresaId = empresaId;
        Telefone = telefone;
        Mensagem = mensagem;
    }
}
