namespace TalentBridge.Application.DTOs.LandingPage;

/// <summary>
/// Dados da landing page de uma empresa
/// </summary>
public class LandingPageResponseDto
{
    public Guid EmpresaId { get; set; }
    public string EmpresaNome { get; set; } = string.Empty;
    public string? EmpresaLogo { get; set; }
    public string? EmpresaWebsite { get; set; }
    public string? EmpresaDescricao { get; set; }
    public string? EmpresaMissao { get; set; }
    public string? EmpresaVisao { get; set; }
    public string? EmpresaValores { get; set; }
    public int TotalVagasAbertas { get; set; }
    public List<VagaLandingPageDto> Vagas { get; set; } = new();
}

/// <summary>
/// Vaga resumida para landing page
/// </summary>
public class VagaLandingPageDto
{
    public Guid VagaId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Cargo { get; set; } = string.Empty;
    public string? DescricaoResumida { get; set; }
    public string Cidade { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public decimal Salario { get; set; }
    public string TipoVaga { get; set; } = string.Empty;
    public string RegimeTrabalho { get; set; } = string.Empty;
    public DateTime DataPublicacao { get; set; }
    public DateTime DataLimite { get; set; }
}

/// <summary>
/// Detalhe completo de uma vaga na landing page
/// </summary>
public class VagaDetalheLandingPageDto
{
    public Guid VagaId { get; set; }
    public Guid EmpresaId { get; set; }
    public string EmpresaNome { get; set; } = string.Empty;
    public string? EmpresaLogo { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Cargo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Atividades { get; set; } = string.Empty;
    public string Beneficios { get; set; } = string.Empty;
    public string DiferenciaisConsiderados { get; set; } = string.Empty;
    public decimal Salario { get; set; }
    public string Cidade { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string TipoVaga { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int QuantidadeVagas { get; set; }
    public DateTime DataPublicacao { get; set; }
    public DateTime DataLimite { get; set; }
    public List<CompetenciaLandingPageDto> Competencias { get; set; } = new();
    public bool AceitaCandidatura { get; set; }
}

/// <summary>
/// Competência resumida para landing page
/// </summary>
public class CompetenciaLandingPageDto
{
    public string Nome { get; set; } = string.Empty;
    public string Nivel { get; set; } = string.Empty;
}
