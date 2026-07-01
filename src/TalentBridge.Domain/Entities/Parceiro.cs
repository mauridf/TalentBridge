using TalentBridge.Domain.Enums;
using TalentBridge.Domain.ValueObjects;

namespace TalentBridge.Domain.Entities;

/// <summary>
/// Parceiro/afiliado do sistema que indica candidatos e empresas
/// </summary>
public class Parceiro : BaseEntity
{
    public string Nome { get; private set; } = string.Empty;
    public string? NomeSocial { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string? Telefone { get; private set; }
    public decimal RendaMensal { get; private set; }
    public string TipoPessoa { get; private set; } = "PF"; // PF ou PJ
    public string Documento { get; private set; } = string.Empty;

    /// <summary>
    /// Código público único do parceiro para links de afiliado
    /// </summary>
    public string CodigoPublico { get; private set; } = string.Empty;
    public string? Origem { get; private set; }
    public StatusParceiroEnum Status { get; private set; } = StatusParceiroEnum.Ativo;

    /// <summary>
    /// ID da wallet no Asaas para split de pagamentos
    /// </summary>
    public string? WalletId { get; private set; }

    /// <summary>
    /// Percentual de split do parceiro (0-100)
    /// </summary>
    public decimal PercentualSplit { get; private set; }

    // Endereço
    public Endereco? Endereco { get; private set; }

    // Relacionamentos
    public ICollection<Cupom> Cupons { get; private set; } = new List<Cupom>();
    public ICollection<Empresa> Empresas { get; private set; } = new List<Empresa>();
    public ICollection<Candidato> Candidatos { get; private set; } = new List<Candidato>();

    protected Parceiro() { }

    public Parceiro(string nome, string email, string documento, string codigoPublico, string tipoPessoa = "PF")
    {
        Nome = nome;
        Email = email.ToLowerInvariant().Trim();
        Documento = documento;
        CodigoPublico = codigoPublico.ToUpperInvariant();
        TipoPessoa = tipoPessoa.ToUpperInvariant();
    }
}
