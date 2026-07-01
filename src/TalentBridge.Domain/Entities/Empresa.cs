using TalentBridge.Domain.ValueObjects;

namespace TalentBridge.Domain.Entities;

/// <summary>
/// Empresa cadastrada no sistema
/// </summary>
public class Empresa : BaseEntity
{
    public string Nome { get; private set; } = string.Empty;
    public string CNPJ { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Telefone { get; private set; } = string.Empty;

    public Guid SegmentoId { get; private set; }
    public Segmento Segmento { get; private set; } = null!;

    // Dados institucionais
    public string? Missao { get; private set; }
    public string? Comentarios { get; private set; }
    public string? Historia { get; private set; }
    public string? Visao { get; private set; }
    public string? Valores { get; private set; }
    public string? Website { get; private set; }
    public string? Whatsapp { get; private set; }
    public string? Instagram { get; private set; }
    public string? Iniciativas { get; private set; }

    // Endereço (Value Object)
    public Endereco? Endereco { get; private set; }

    // Configurações
    public bool DesativarNotificacoes { get; private set; }
    public bool ReceberNotificacoesCandidaturas { get; private set; } = true;
    public bool EnviarEmailNovoLoginNavegador { get; private set; } = true;
    public bool EnviarEmailAtividadeIncomum { get; private set; } = true;

    // Relacionamentos
    public Guid? ParceiroId { get; private set; }
    public Parceiro? Parceiro { get; private set; }

    public ICollection<Gestor> Gestores { get; private set; } = new List<Gestor>();
    public ICollection<Recrutador> Recrutadores { get; private set; } = new List<Recrutador>();
    public ICollection<Vaga> Vagas { get; private set; } = new List<Vaga>();
    public ICollection<Convite> Convites { get; private set; } = new List<Convite>();
    public ICollection<Competencia> Competencias { get; private set; } = new List<Competencia>();
    public ICollection<UsuarioEmpresa> UsuarioEmpresas { get; private set; } = new List<UsuarioEmpresa>();
    public ICollection<CreditosEmpresa> CreditosEmpresa { get; private set; } = new List<CreditosEmpresa>();

    protected Empresa() { }

    public Empresa(string nome, string cnpj, string email, string telefone, Guid segmentoId)
    {
        Nome = nome;
        CNPJ = cnpj?.Replace(".", "").Replace("-", "").Replace("/", "") ?? string.Empty;
        Email = email.ToLowerInvariant().Trim();
        Telefone = telefone;
        SegmentoId = segmentoId;
    }

    /// <summary>
    /// Atualiza endereço da empresa
    /// </summary>
    public void AtualizarEndereco(Endereco? endereco)
    {
        Endereco = endereco;
        AtualizarDataModificacao();
    }

    /// <summary>
    /// Atualiza configurações de notificação
    /// </summary>
    public void AtualizarConfiguracoesNotificacao(
        bool desativarNotificacoes,
        bool receberNotificacoesCandidaturas,
        bool enviarEmailNovoLoginNavegador,
        bool enviarEmailAtividadeIncomum)
    {
        DesativarNotificacoes = desativarNotificacoes;
        ReceberNotificacoesCandidaturas = receberNotificacoesCandidaturas;
        EnviarEmailNovoLoginNavegador = enviarEmailNovoLoginNavegador;
        EnviarEmailAtividadeIncomum = enviarEmailAtividadeIncomum;
        AtualizarDataModificacao();
    }

    /// <summary>
    /// Retorna o slug da empresa para URLs amigáveis
    /// </summary>
    public string GerarSlug()
    {
        return Nome.ToLowerInvariant()
            .Replace(" ", "-")
            .Replace("ç", "c")
            .Replace("ã", "a")
            .Replace("á", "a")
            .Replace("é", "e")
            .Replace("í", "i")
            .Replace("ó", "o")
            .Replace("ú", "u");
    }
}
