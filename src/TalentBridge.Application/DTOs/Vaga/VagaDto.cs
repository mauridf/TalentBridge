using TalentBridge.Domain.Enums;

namespace TalentBridge.Application.DTOs.Vaga;

/// <summary>
/// Requisição para criar ou editar uma vaga (upsert)
/// </summary>
public class VagaUpsertRequestDto
{
    /// <summary>
    /// ID da vaga (null para criar, preenchido para editar)
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// Título da vaga
    /// </summary>
    public string Titulo { get; set; } = string.Empty;

    /// <summary>
    /// Cargo
    /// </summary>
    public string Cargo { get; set; } = string.Empty;

    /// <summary>
    /// Descrição detalhada
    /// </summary>
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// Atividades (separadas por ;)
    /// </summary>
    public string Atividades { get; set; } = string.Empty;

    /// <summary>
    /// Benefícios (separados por ;)
    /// </summary>
    public string Beneficios { get; set; } = string.Empty;

    /// <summary>
    /// Diferenciais considerados
    /// </summary>
    public string DiferenciaisConsiderados { get; set; } = string.Empty;

    /// <summary>
    /// Salário
    /// </summary>
    public decimal Salario { get; set; }

    /// <summary>
    /// Regime de trabalho (código do domínio)
    /// </summary>
    public int RegimeTrabalho { get; set; }

    /// <summary>
    /// Jornada de trabalho (código do domínio)
    /// </summary>
    public int JornadaTrabalho { get; set; }

    /// <summary>
    /// Tipo de contratação (código do domínio)
    /// </summary>
    public int TipoContratacao { get; set; }

    /// <summary>
    /// Formação acadêmica exigida (código do domínio)
    /// </summary>
    public int FormacaoAcademica { get; set; }

    /// <summary>
    /// Formação acadêmica é eliminatória?
    /// </summary>
    public bool FormacaoAcademicaEliminatorio { get; set; }

    /// <summary>
    /// Área de atuação (código do domínio)
    /// </summary>
    public int AreaAtuacao { get; set; }

    /// <summary>
    /// Tempo de experiência exigido (código do domínio)
    /// </summary>
    public int TempoExperiencia { get; set; }

    /// <summary>
    /// Tempo de experiência é eliminatório?
    /// </summary>
    public bool TempoExperienciaEliminatorio { get; set; }

    /// <summary>
    /// Ações afirmativas (códigos separados por ;)
    /// </summary>
    public string? AcoesAfirmativas { get; set; }

    /// <summary>
    /// Idade mínima
    /// </summary>
    public int? IdadeMinima { get; set; }

    /// <summary>
    /// Idade máxima
    /// </summary>
    public int? IdadeMaxima { get; set; }

    /// <summary>
    /// Exige ocupação anterior no cargo?
    /// </summary>
    public bool OcupacaoAnteriorCargo { get; set; }

    /// <summary>
    /// Exige disponibilidade para deslocamento?
    /// </summary>
    public bool DisponibilidadeDeslocamento { get; set; }

    // Localização
    public string Estado { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string? CEP { get; set; }
    public string? Rua { get; set; }
    public string? Numero { get; set; }
    public string? Bairro { get; set; }
    public string? Complemento { get; set; }
    public bool UtilizaEnderecoCadastrado { get; set; }

    // Datas
    public DateTime DataCandidaturaInicio { get; set; }
    public DateTime DataCandidaturaFim { get; set; }

    // Tipo e Configurações
    public TipoVagaEnum TipoVaga { get; set; } = TipoVagaEnum.Interna;
    public string? LinkExterno { get; set; }
    public int QuantidadeVagas { get; set; } = 1;
    public bool RecrutamentoWhatsApp { get; set; }

    // Big Five
    public int? ExtroversaoMinima { get; set; }
    public int? AmabilidadeMinima { get; set; }
    public int? AutodisciplinaMinima { get; set; }
    public int? EstabilidadeEmocionalMinima { get; set; }
    public int? AberturaExperienciaMinima { get; set; }

    // Competências
    public List<CompetenciaVagaDto>? Competencias { get; set; }
}

/// <summary>
/// Competência associada a uma vaga
/// </summary>
public class CompetenciaVagaDto
{
    public Guid CompetenciaId { get; set; }
    public int Nivel { get; set; }
    public int Peso { get; set; }
}

/// <summary>
/// Filtros para busca de vagas
/// </summary>
public class BuscarVagasRequestDto
{
    public string? Termo { get; set; }
    public string? Cidade { get; set; }
    public string? Estado { get; set; }
    public int? AreaAtuacao { get; set; }
    public int? RegimeTrabalho { get; set; }
    public int? TipoContratacao { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

/// <summary>
/// Resposta com dados da vaga
/// </summary>
public class VagaResponseDto
{
    public Guid Id { get; set; }
    public Guid EmpresaId { get; set; }
    public string EmpresaNome { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string Cargo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Atividades { get; set; } = string.Empty;
    public string Beneficios { get; set; } = string.Empty;
    public decimal Salario { get; set; }
    public string Cidade { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string TipoVaga { get; set; } = string.Empty;
    public int QuantidadeVagas { get; set; }
    public DateTime DataCandidaturaInicio { get; set; }
    public DateTime DataCandidaturaFim { get; set; }
    public bool Encerrada { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<CompetenciaVagaResponseDto>? Competencias { get; set; }
}

/// <summary>
/// Competência na resposta da vaga
/// </summary>
public class CompetenciaVagaResponseDto
{
    public Guid CompetenciaId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Nivel { get; set; } = string.Empty;
    public int Peso { get; set; }
}
