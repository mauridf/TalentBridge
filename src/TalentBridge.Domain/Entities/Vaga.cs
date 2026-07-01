using System.ComponentModel;
using TalentBridge.Domain.Enums;
using TalentBridge.Domain.ValueObjects;

namespace TalentBridge.Domain.Entities;

/// <summary>
/// Vaga de emprego publicada por uma empresa
/// </summary>
public class Vaga : BaseEntity
{
    // ==========================================
    // Identificação e Empresa
    // ==========================================
    public Guid EmpresaId { get; private set; }
    public Empresa Empresa { get; private set; } = null!;

    public Guid UsuarioCriacaoId { get; private set; }
    public Usuario UsuarioCriacao { get; private set; } = null!;

    public Guid? UsuarioEncerramentoId { get; private set; }
    public Usuario? UsuarioEncerramento { get; private set; }

    // ==========================================
    // Dados Básicos
    // ==========================================
    public string Titulo { get; private set; } = string.Empty;
    public string Cargo { get; private set; } = string.Empty;
    public string Descricao { get; private set; } = string.Empty;
    public string Atividades { get; private set; } = string.Empty;
    public string Beneficios { get; private set; } = string.Empty;
    public string DiferenciaisConsiderados { get; private set; } = string.Empty;
    public decimal Salario { get; private set; }

    // ==========================================
    // Requisitos
    // ==========================================
    public int RegimeTrabalho { get; private set; }
    public int JornadaTrabalho { get; private set; }
    public int TipoContratacao { get; private set; }
    public int FormacaoAcademica { get; private set; }
    public bool FormacaoAcademicaEliminatorio { get; private set; }
    public int AreaAtuacao { get; private set; }
    public int TempoExperiencia { get; private set; }
    public bool TempoExperienciaEliminatorio { get; private set; }

    // Ações Afirmativas
    public string? AcoesAfirmativas { get; private set; }

    // Idade
    public int? IdadeMinima { get; private set; }
    public int? IdadeMaxima { get; private set; }

    // Outros requisitos
    public bool OcupacaoAnteriorCargo { get; private set; }
    public bool DisponibilidadeDeslocamento { get; private set; }

    // ==========================================
    // Localização
    // ==========================================
    public string Estado { get; private set; } = string.Empty;
    public string Cidade { get; private set; } = string.Empty;
    public string? CEP { get; private set; }
    public string? Rua { get; private set; }
    public string? Numero { get; private set; }
    public string? Bairro { get; private set; }
    public string? Complemento { get; private set; }
    public bool UtilizaEnderecoCadastrado { get; private set; }
    public double? Latitude { get; private set; }
    public double? Longitude { get; private set; }

    // ==========================================
    // Status e Datas
    // ==========================================
    public StatusVagaEnum Status { get; private set; } = StatusVagaEnum.Aberta;
    public bool Encerrada { get; private set; }
    public DateTime DataCandidaturaInicio { get; private set; }
    public DateTime DataCandidaturaFim { get; private set; }
    public DateTime? DataEncerramento { get; private set; }

    // ==========================================
    // Tipo e Configurações
    // ==========================================
    public TipoVagaEnum TipoVaga { get; private set; } = TipoVagaEnum.Interna;
    public string? LinkExterno { get; private set; }
    public int QuantidadeVagas { get; private set; } = 1;
    public bool RecrutamentoWhatsApp { get; private set; }
    public DateTime? DataRecrutamentoWhatsApp { get; private set; }
    public DateTime? DataPublicacaoGoogle { get; private set; }

    // ==========================================
    // Requisitos Big Five
    // ==========================================
    public int? ExtroversaoMinima { get; private set; }
    public int? AmabilidadeMinima { get; private set; }
    public int? AutodisciplinaMinima { get; private set; }
    public int? EstabilidadeEmocionalMinima { get; private set; }
    public int? AberturaExperienciaMinima { get; private set; }

    // ==========================================
    // Relacionamentos
    // ==========================================
    public ICollection<CompetenciaVaga> CompetenciasVagas { get; private set; } = new List<CompetenciaVaga>();
    public ICollection<Candidatura> Candidaturas { get; private set; } = new List<Candidatura>();
    public ICollection<Visita> Visitas { get; private set; } = new List<Visita>();
    public ICollection<CreditoVagas> CreditoVagas { get; private set; } = new List<CreditoVagas>();

    protected Vaga() { }

    private Vaga(
        Guid empresaId,
        Guid usuarioCriacaoId,
        string titulo,
        string cargo,
        string descricao,
        decimal salario,
        DateTime dataCandidaturaInicio,
        DateTime dataCandidaturaFim)
    {
        EmpresaId = empresaId;
        UsuarioCriacaoId = usuarioCriacaoId;
        Titulo = titulo;
        Cargo = cargo;
        Descricao = descricao;
        Salario = salario;
        DataCandidaturaInicio = dataCandidaturaInicio;
        DataCandidaturaFim = dataCandidaturaFim;
        Status = StatusVagaEnum.Aberta;
    }

    /// <summary>
    /// Factory method para criar uma nova vaga
    /// </summary>
    public static Vaga Criar(
        Guid empresaId,
        Guid usuarioCriacaoId,
        string titulo,
        string cargo,
        string descricao,
        decimal salario,
        DateTime dataInicio,
        DateTime dataFim)
    {
        return new Vaga(empresaId, usuarioCriacaoId, titulo, cargo, descricao, salario, dataInicio, dataFim);
    }

    /// <summary>
    /// Encerra a vaga
    /// </summary>
    public void Encerrar(Guid usuarioEncerramentoId)
    {
        Encerrada = true;
        Status = StatusVagaEnum.Fechada;
        UsuarioEncerramentoId = usuarioEncerramentoId;
        DataEncerramento = DateTime.UtcNow;
        AtualizarDataModificacao();
    }

    /// <summary>
    /// Reativa a vaga
    /// </summary>
    public void Reativar()
    {
        Encerrada = false;
        Status = StatusVagaEnum.Aberta;
        DataEncerramento = null;
        UsuarioEncerramentoId = null;
        AtualizarDataModificacao();
    }

    /// <summary>
    /// Verifica se a vaga está dentro do período de candidatura
    /// </summary>
    public bool EstaNoPeriodoCandidatura()
    {
        var hoje = DateTime.UtcNow;
        return !Encerrada &&
               Status == StatusVagaEnum.Aberta &&
               hoje >= DataCandidaturaInicio &&
               hoje <= DataCandidaturaFim;
    }

    /// <summary>
    /// Gera slug para URL amigável
    /// </summary>
    public string GerarSlug()
    {
        return $"{Titulo.ToLowerInvariant().Replace(" ", "-")}-{Id.ToString()[..8]}";
    }
}
