using TalentBridge.Domain.Enums;

namespace TalentBridge.Domain.Entities;

/// <summary>
/// Tabela de domínio (lookup table) para valores padronizados do sistema.
/// Ex: Regime de Trabalho, Tipo de Contratação, Jornada, etc.
/// </summary>
public class Dominio : BaseEntity
{
    /// <summary>
    /// Código único dentro do tipo (compõe chave alternativa com Tipo)
    /// </summary>
    public int Codigo { get; private set; }

    /// <summary>
    /// Descrição do valor do domínio
    /// </summary>
    public string Descricao { get; private set; } = string.Empty;

    /// <summary>
    /// Tipo do domínio (RegimeTrabalho, TipoContratacao, etc.)
    /// </summary>
    public TipoDominioEnum Tipo { get; private set; }

    /// <summary>
    /// Status do registro (Ativo ou Inativo)
    /// </summary>
    public StatusComumEnum Status { get; private set; } = StatusComumEnum.Ativo;

    /// <summary>
    /// Data de inativação (se aplicável)
    /// </summary>
    public DateTime? DataInativacao { get; private set; }

    /// <summary>
    /// Usuário que inativou o registro
    /// </summary>
    public Guid? UsuarioInativacaoId { get; private set; }

    protected Dominio() { }

    public Dominio(int codigo, string descricao, TipoDominioEnum tipo)
    {
        Codigo = codigo;
        Descricao = descricao;
        Tipo = tipo;
    }

    /// <summary>
    /// Inativa o registro de domínio
    /// </summary>
    public void Inativar(Guid usuarioInativacaoId)
    {
        Status = StatusComumEnum.Inativo;
        DataInativacao = DateTime.UtcNow;
        UsuarioInativacaoId = usuarioInativacaoId;
        AtualizarDataModificacao();
    }

    /// <summary>
    /// Ativa o registro de domínio
    /// </summary>
    public void Ativar()
    {
        Status = StatusComumEnum.Ativo;
        DataInativacao = null;
        UsuarioInativacaoId = null;
        AtualizarDataModificacao();
    }
}
