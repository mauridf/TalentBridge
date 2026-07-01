namespace TalentBridge.Application.DTOs.Empresa;

/// <summary>
/// Requisição para criar uma empresa com gestor (via convite)
/// </summary>
public class CriarEmpresaRequestDto
{
    /// <summary>
    /// Token do convite
    /// </summary>
    public Guid TokenConvite { get; set; }

    /// <summary>
    /// Nome do gestor
    /// </summary>
    public string NomeGestor { get; set; } = string.Empty;

    /// <summary>
    /// Email do gestor
    /// </summary>
    public string EmailGestor { get; set; } = string.Empty;

    /// <summary>
    /// Senha do gestor
    /// </summary>
    public string Senha { get; set; } = string.Empty;

    /// <summary>
    /// Confirmação de senha
    /// </summary>
    public string ConfirmacaoSenha { get; set; } = string.Empty;

    /// <summary>
    /// Nome da empresa
    /// </summary>
    public string NomeEmpresa { get; set; } = string.Empty;

    /// <summary>
    /// CNPJ da empresa (apenas números)
    /// </summary>
    public string Cnpj { get; set; } = string.Empty;

    /// <summary>
    /// Telefone da empresa
    /// </summary>
    public string TelefoneEmpresa { get; set; } = string.Empty;

    /// <summary>
    /// ID do segmento
    /// </summary>
    public Guid SegmentoId { get; set; }
}

/// <summary>
/// Resposta após criar empresa
/// </summary>
public class CriarEmpresaResponseDto
{
    public Guid EmpresaId { get; set; }
    public Guid GestorId { get; set; }
    public string NomeEmpresa { get; set; } = string.Empty;
    public string NomeGestor { get; set; } = string.Empty;
    public string Mensagem { get; set; } = string.Empty;
}
