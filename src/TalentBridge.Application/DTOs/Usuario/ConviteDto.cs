namespace TalentBridge.Application.DTOs.Usuario;

/// <summary>
/// Requisição para criar um convite
/// </summary>
public class CriarConviteRequestDto
{
    /// <summary>
    /// Email do convidado
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de convite (0=Recrutador, 1=Empresa)
    /// </summary>
    public int Tipo { get; set; }

    /// <summary>
    /// CNPJ da empresa (para convite de empresa)
    /// </summary>
    public string? Cnpj { get; set; }

    /// <summary>
    /// Nome da empresa
    /// </summary>
    public string? NomeEmpresa { get; set; }

    /// <summary>
    /// Nome do responsável
    /// </summary>
    public string? NomeResponsavel { get; set; }

    /// <summary>
    /// Telefone de contato
    /// </summary>
    public string? Telefone { get; set; }
}

/// <summary>
/// Resposta com dados do convite
/// </summary>
public class ConviteResponseDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public Guid Token { get; set; }
    public DateTime DataExpiracao { get; set; }
    public DateTime? DataAceite { get; set; }
}

/// <summary>
/// Requisição para validar token de convite
/// </summary>
public class ValidarConviteRequestDto
{
    public Guid Token { get; set; }
}
