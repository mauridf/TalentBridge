namespace TalentBridge.Domain.Entities;

/// <summary>
/// Histórico de transações de créditos da empresa
/// </summary>
public class HistoricoTransacao : BaseEntity
{
    public Guid EmpresaId { get; private set; }
    public Empresa Empresa { get; private set; } = null!;

    /// <summary>
    /// CNPJ da empresa no momento da transação
    /// </summary>
    public string CnpjEmpresa { get; private set; } = string.Empty;

    public Guid? VagaId { get; private set; }
    public Vaga? Vaga { get; private set; }

    public Guid? CreditoEmpresaId { get; private set; }
    public CreditosEmpresa? CreditoEmpresa { get; private set; }

    public Guid? CreditoVagaId { get; private set; }
    public CreditoVagas? CreditoVaga { get; private set; }

    /// <summary>
    /// Descrição da transação
    /// </summary>
    public string DescricaoTransacao { get; private set; } = string.Empty;

    /// <summary>
    /// Perfil do responsável pela transação
    /// </summary>
    public string PerfilResponsavel { get; private set; } = string.Empty;

    /// <summary>
    /// Data da transação
    /// </summary>
    public DateTime DataTransacao { get; private set; }

    /// <summary>
    /// Valor pago na transação (se aplicável)
    /// </summary>
    public decimal? ValorPago { get; private set; }

    /// <summary>
    /// Alteração na quantidade de créditos
    /// </summary>
    public int? AlteracaoCredito { get; private set; }

    protected HistoricoTransacao() { }

    public HistoricoTransacao(
        Guid empresaId,
        string cnpjEmpresa,
        string descricaoTransacao,
        string perfilResponsavel,
        Guid? vagaId = null,
        Guid? creditoEmpresaId = null,
        Guid? creditoVagaId = null,
        decimal? valorPago = null,
        int? alteracaoCredito = null)
    {
        EmpresaId = empresaId;
        CnpjEmpresa = cnpjEmpresa;
        DescricaoTransacao = descricaoTransacao;
        PerfilResponsavel = perfilResponsavel;
        DataTransacao = DateTime.UtcNow;
        VagaId = vagaId;
        CreditoEmpresaId = creditoEmpresaId;
        CreditoVagaId = creditoVagaId;
        ValorPago = valorPago;
        AlteracaoCredito = alteracaoCredito;
    }
}
