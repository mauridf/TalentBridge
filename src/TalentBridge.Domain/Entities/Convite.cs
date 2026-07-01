using TalentBridge.Domain.Enums;

namespace TalentBridge.Domain.Entities;

/// <summary>
/// Convite para acesso ao sistema (Gestor ou Recrutador)
/// </summary>
public class Convite : BaseEntity
{
    /// <summary>
    /// Email do convidado
    /// </summary>
    public string Email { get; private set; } = string.Empty;

    /// <summary>
    /// CNPJ da empresa
    /// </summary>
    public string? Cnpj { get; private set; }

    /// <summary>
    /// Nome da empresa
    /// </summary>
    public string? NomeEmpresa { get; private set; }

    /// <summary>
    /// Nome do responsável
    /// </summary>
    public string? NomeResponsavel { get; private set; }

    /// <summary>
    /// Telefone de contato
    /// </summary>
    public string? Telefone { get; private set; }

    /// <summary>
    /// Status do convite
    /// </summary>
    public StatusConviteEnum Status { get; private set; } = StatusConviteEnum.Pendente;

    /// <summary>
    /// Tipo de convite (Recrutador ou Empresa)
    /// </summary>
    public TipoConviteEnum Tipo { get; private set; }

    /// <summary>
    /// Token único para aceite do convite
    /// </summary>
    public Guid Token { get; private set; }

    /// <summary>
    /// Data de expiração do convite
    /// </summary>
    public DateTime DataExpiracao { get; private set; }

    /// <summary>
    /// Data em que o convite foi aceito
    /// </summary>
    public DateTime? DataAceite { get; private set; }

    /// <summary>
    /// Empresa responsável pelo convite
    /// </summary>
    public Guid EmpresaResponsavelId { get; private set; }
    public Empresa EmpresaResponsavel { get; private set; } = null!;

    // Relacionamento
    public Recrutador? Recrutador { get; private set; }

    protected Convite() { }

    public Convite(string email, TipoConviteEnum tipo, Guid empresaResponsavelId, int diasExpiracao = 7)
    {
        Email = email.ToLowerInvariant().Trim();
        Tipo = tipo;
        EmpresaResponsavelId = empresaResponsavelId;
        Token = Guid.NewGuid();
        DataExpiracao = DateTime.UtcNow.AddDays(diasExpiracao);
    }

    /// <summary>
    /// Aceita o convite
    /// </summary>
    public void Aceitar()
    {
        Status = StatusConviteEnum.Aceito;
        DataAceite = DateTime.UtcNow;
        AtualizarDataModificacao();
    }

    /// <summary>
    /// Inativa o convite
    /// </summary>
    public void Inativar()
    {
        Status = StatusConviteEnum.Inativo;
        AtualizarDataModificacao();
    }

    /// <summary>
    /// Verifica se o convite ainda é válido
    /// </summary>
    public bool EstaValido()
    {
        return Status == StatusConviteEnum.Pendente && DateTime.UtcNow <= DataExpiracao;
    }
}
