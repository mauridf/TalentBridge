namespace TalentBridge.Domain.Entities;

/// <summary>
/// Usuário do tipo Recrutador (herda de Usuario via TPH)
/// </summary>
public class Recrutador : Usuario
{
    /// <summary>
    /// Nome social do recrutador (opcional)
    /// </summary>
    public string? NomeSocial { get; private set; }

    /// <summary>
    /// Empresa que o recrutador pertence
    /// </summary>
    public Guid EmpresaId { get; private set; }
    public Empresa Empresa { get; private set; } = null!;

    /// <summary>
    /// Convite que originou o cadastro
    /// </summary>
    public Guid ConviteId { get; private set; }
    public Convite Convite { get; private set; } = null!;

    protected Recrutador() { }

    public Recrutador(string nome, string email, string senhaHash, Guid perfilId,
        Guid empresaId, Guid conviteId, string? nomeSocial = null)
        : base(nome, email, senhaHash, perfilId, "Recrutador")
    {
        EmpresaId = empresaId;
        ConviteId = conviteId;
        NomeSocial = nomeSocial;
    }
}
