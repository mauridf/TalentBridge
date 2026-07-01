using TalentBridge.Domain.ValueObjects;

namespace TalentBridge.Domain.Entities;

/// <summary>
/// Perfil pessoal do candidato (dados demográficos e sociais)
/// </summary>
public class PerfilPessoal : BaseEntity
{
    /// <summary>
    /// Cor/Raça (FK → Dominios)
    /// </summary>
    public int? CorRaca { get; set; }

    /// <summary>
    /// Pronome de tratamento (FK → Dominios)
    /// </summary>
    public int? Pronome { get; set; }

    /// <summary>
    /// Identidade de gênero (FK → Dominios)
    /// </summary>
    public int? IdentidadeGenero { get; set; }

    /// <summary>
    /// Orientação sexual (FK → Dominios)
    /// </summary>
    public int? OrientacaoSexual { get; set; }

    /// <summary>
    /// CPF do candidato
    /// </summary>
    public string? Cpf { get; set; }

    /// <summary>
    /// RG do candidato
    /// </summary>
    public string? Rg { get; set; }

    /// <summary>
    /// Texto "Sobre mim" do candidato
    /// </summary>
    public string? SobreMim { get; set; }

    /// <summary>
    /// Local de residência
    /// </summary>
    public string? LocalResidencia { get; set; }

    /// <summary>
    /// Códigos de ações afirmativas separados por ;
    /// </summary>
    public string? AcoesAfirmativas { get; set; }

    /// <summary>
    /// Descrição de PCD (Pessoa com Deficiência)
    /// </summary>
    public string? DescricaoPcd { get; set; }

    // Redes sociais
    public string? Instagram { get; set; }
    public string? Facebook { get; set; }
    public string? Linkedin { get; set; }

    // Endereço (Value Object)
    public Endereco? Endereco { get; set; }

    // Relacionamento 1:1
    public Candidato? Candidato { get; set; }

    protected PerfilPessoal() { }

    public PerfilPessoal(string? sobreMim)
    {
        SobreMim = sobreMim;
    }

    /// <summary>
    /// Atualiza endereço do perfil pessoal
    /// </summary>
    public void AtualizarEndereco(Endereco? endereco)
    {
        Endereco = endereco;
        AtualizarDataModificacao();
    }
}
