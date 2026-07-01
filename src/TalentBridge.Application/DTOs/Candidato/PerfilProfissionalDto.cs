namespace TalentBridge.Application.DTOs.Candidato;

/// <summary>
/// Requisição para criar/editar perfil profissional (upsert)
/// </summary>
public class PerfilProfissionalUpsertRequestDto
{
    /// <summary>
    /// Dispensa experiência profissional?
    /// </summary>
    public bool DispensaExperienciaProfissional { get; set; }

    /// <summary>
    /// Competências do candidato
    /// </summary>
    public List<CompetenciaCandidatoRequestDto>? Competencias { get; set; }

    /// <summary>
    /// Formações acadêmicas
    /// </summary>
    public List<FormacaoAcademicaRequestDto>? FormacoesAcademicas { get; set; }

    /// <summary>
    /// Experiências profissionais
    /// </summary>
    public List<ExperienciaProfissionalRequestDto>? ExperienciasProfissionais { get; set; }

    /// <summary>
    /// Áreas de interesse (códigos)
    /// </summary>
    public List<int>? AreasInteresse { get; set; }
}

/// <summary>
/// Competência do candidato na requisição
/// </summary>
public class CompetenciaCandidatoRequestDto
{
    public Guid CompetenciaId { get; set; }
    public int Nivel { get; set; }
}

/// <summary>
/// Formação acadêmica na requisição
/// </summary>
public class FormacaoAcademicaRequestDto
{
    public string Grau { get; set; } = string.Empty;
    public string AreaAtuacao { get; set; } = string.Empty;
    public DateTime? DataConclusao { get; set; }
    public bool Concluido { get; set; }
}

/// <summary>
/// Experiência profissional na requisição
/// </summary>
public class ExperienciaProfissionalRequestDto
{
    public string Empresa { get; set; } = string.Empty;
    public string Posicao { get; set; } = string.Empty;
    public DateTime DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public bool EmpregoAtual { get; set; }
}

/// <summary>
/// Resposta com dados do perfil profissional
/// </summary>
public class PerfilProfissionalResponseDto
{
    public Guid Id { get; set; }
    public bool DispensaExperienciaProfissional { get; set; }
    public List<CompetenciaCandidatoResponseDto>? Competencias { get; set; }
    public List<FormacaoAcademicaResponseDto>? FormacoesAcademicas { get; set; }
    public List<ExperienciaProfissionalResponseDto>? ExperienciasProfissionais { get; set; }
    public List<int>? AreasInteresse { get; set; }
}

public class CompetenciaCandidatoResponseDto
{
    public Guid CompetenciaId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Nivel { get; set; } = string.Empty;
}

public class FormacaoAcademicaResponseDto
{
    public Guid Id { get; set; }
    public string Grau { get; set; } = string.Empty;
    public string AreaAtuacao { get; set; } = string.Empty;
    public DateTime? DataConclusao { get; set; }
    public bool Concluido { get; set; }
}

public class ExperienciaProfissionalResponseDto
{
    public Guid Id { get; set; }
    public string Empresa { get; set; } = string.Empty;
    public string Posicao { get; set; } = string.Empty;
    public DateTime DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public bool EmpregoAtual { get; set; }
}
