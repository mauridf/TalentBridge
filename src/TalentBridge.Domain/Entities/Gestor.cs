namespace TalentBridge.Domain.Entities;

/// <summary>
/// Usuário do tipo Gestor de Empresa (herda de Usuario via TPH)
/// </summary>
public class Gestor : Usuario
{
    /// <summary>
    /// Empresa que o gestor gerencia
    /// </summary>
    public Guid EmpresaId { get; private set; }
    public Empresa Empresa { get; private set; } = null!;

    protected Gestor() { }

    public Gestor(string nome, string email, string senhaHash, Guid perfilId, Guid empresaId)
        : base(nome, email, senhaHash, perfilId, "Gestor")
    {
        EmpresaId = empresaId;
    }
}
