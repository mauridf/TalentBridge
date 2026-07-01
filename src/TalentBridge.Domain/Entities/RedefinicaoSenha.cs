namespace TalentBridge.Domain.Entities;

/// <summary>
/// Token para redefinição de senha do usuário
/// </summary>
public class RedefinicaoSenha : BaseEntity
{
    public Guid UsuarioId { get; private set; }
    public Usuario Usuario { get; private set; } = null!;

    /// <summary>
    /// Token de recuperação
    /// </summary>
    public string Token { get; private set; } = string.Empty;

    /// <summary>
    /// Data de expiração do token (padrão: 3 minutos)
    /// </summary>
    public DateTime DataExpiracao { get; private set; }

    /// <summary>
    /// Indica se o token já foi utilizado
    /// </summary>
    public bool Utilizado { get; private set; }

    protected RedefinicaoSenha() { }

    public RedefinicaoSenha(Guid usuarioId, string token, int minutosExpiracao = 3)
    {
        UsuarioId = usuarioId;
        Token = token;
        DataExpiracao = DateTime.UtcNow.AddMinutes(minutosExpiracao);
        Utilizado = false;
    }

    /// <summary>
    /// Verifica se o token é válido
    /// </summary>
    public bool EstaValido()
    {
        return !Utilizado && DateTime.UtcNow <= DataExpiracao;
    }

    /// <summary>
    /// Marca o token como utilizado
    /// </summary>
    public void MarcarUtilizado()
    {
        Utilizado = true;
        AtualizarDataModificacao();
    }
}
