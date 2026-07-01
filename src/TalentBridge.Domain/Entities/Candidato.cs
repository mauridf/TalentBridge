using System.ComponentModel;

namespace TalentBridge.Domain.Entities;

/// <summary>
/// Usuário do tipo Candidato (herda de Usuario via TPH)
/// </summary>
public class Candidato : Usuario
{
    /// <summary>
    /// Nome social do candidato (opcional)
    /// </summary>
    public string? NomeSocial { get; set; }

    /// <summary>
    /// Data de nascimento
    /// </summary>
    public DateTime DataNascimento { get; private set; }

    /// <summary>
    /// Token para confirmação de email
    /// </summary>
    public Guid? TokenConfirmacaoEmail { get; private set; }

    /// <summary>
    /// Pontuações do teste Big Five (5 traços)
    /// </summary>
    public int? Extroversao { get; private set; }
    public int? Amabilidade { get; private set; }
    public int? Consciencia { get; private set; }
    public int? EstabilidadeEmocional { get; private set; }
    public int? AberturaExperiencia { get; private set; }

    /// <summary>
    /// Data do último teste Big Five realizado
    /// </summary>
    public DateTime? DataUltimoTesteBigFive { get; private set; }

    // Relacionamentos
    public Guid? PerfilPessoalId { get; set; }
    public PerfilPessoal? PerfilPessoal { get; private set; }

    public Guid? PerfilProfissionalId { get; set; }
    public PerfilProfissional? PerfilProfissional { get; private set; }

    public Guid? ParceiroId { get; set; }
    public Parceiro? Parceiro { get; private set; }

    public ICollection<Candidatura> Candidaturas { get; private set; } = new List<Candidatura>();
    public ICollection<Visita> Visitas { get; private set; } = new List<Visita>();

    // Construtor para EF Core
    protected Candidato() { }

    public Candidato(string nome, string email, string senhaHash, Guid perfilId, DateTime dataNascimento)
        : base(nome, email, senhaHash, perfilId, "Candidato")
    {
        DataNascimento = dataNascimento;
        TokenConfirmacaoEmail = Guid.NewGuid();
    }

    /// <summary>
    /// Atualiza dados específicos do candidato
    /// </summary>
    public void AtualizarDadosCandidato(string? nomeSocial, DateTime dataNascimento)
    {
        NomeSocial = nomeSocial;
        DataNascimento = dataNascimento;
        AtualizarDataModificacao();
    }

    /// <summary>
    /// Salva resultado do teste Big Five
    /// </summary>
    public void SalvarTesteBigFive(int extroversao, int amabilidade, int consciencia,
        int estabilidadeEmocional, int aberturaExperiencia)
    {
        Extroversao = extroversao;
        Amabilidade = amabilidade;
        Consciencia = consciencia;
        EstabilidadeEmocional = estabilidadeEmocional;
        AberturaExperiencia = aberturaExperiencia;
        DataUltimoTesteBigFive = DateTime.UtcNow;
        AtualizarDataModificacao();
    }

    /// <summary>
    /// Verifica se o candidato já realizou o teste Big Five
    /// </summary>
    public bool RealizouTesteBigFive() => DataUltimoTesteBigFive.HasValue;

    /// <summary>
    /// Confirma o email do candidato
    /// </summary>
    public void ConfirmarEmail()
    {
        TokenConfirmacaoEmail = null;
        Ativar();
    }

    /// <summary>
    /// Gera novo token de confirmação de email
    /// </summary>
    public void GerarNovoTokenConfirmacao()
    {
        TokenConfirmacaoEmail = Guid.NewGuid();
        AtualizarDataModificacao();
    }
}
