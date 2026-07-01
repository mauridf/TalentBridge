namespace TalentBridge.Domain.Entities;

/// <summary>
/// Associação entre Usuário e Empresa com perfil.
/// Permite que um usuário tenha múltiplos perfis em múltiplas empresas.
/// </summary>
public class UsuarioEmpresa : BaseEntity
{
    public Guid UsuarioId { get; private set; }
    public Usuario Usuario { get; private set; } = null!;

    public Guid EmpresaId { get; private set; }
    public Empresa Empresa { get; private set; } = null!;

    public Guid PerfilId { get; private set; }
    public Perfil Perfil { get; private set; } = null!;

    protected UsuarioEmpresa() { }

    public UsuarioEmpresa(Guid usuarioId, Guid empresaId, Guid perfilId)
    {
        UsuarioId = usuarioId;
        EmpresaId = empresaId;
        PerfilId = perfilId;
    }
}
