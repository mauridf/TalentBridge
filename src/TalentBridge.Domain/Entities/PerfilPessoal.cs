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
    public int? CorRaca { get; private set; }

    /// <summary>
    /// Pronome de tratamento (FK → Dominios)
    /// </summary>
    public int? Pronome { get; private set; }

    /// <summary>
    /// Identidade de gênero (FK → Dominios)
    /// </summary>
    public int? IdentidadeGenero { get; private set; }

    /// <summary>
    /// Orientação sexual (FK → Dominios)
    /// </summary>
    public int? OrientacaoSexual { get; private set; }

    /// <summary>
    /// CPF do candidato
    /// </summary>
    public string? Cpf { get; private set; }

    /// <summary>
    /// RG do candidato
    /// </summary>
    public string? Rg { get; private set; }

    /// <summary>
    /// Texto "Sobre mim" do candidato
    /// </summary>
    public string? SobreMim { get; private set; }

    /// <summary>
    /// Local de residência
    /// </summary>
    public string? LocalResidencia { get; private set; }

    /// <summary>
    /// Códigos de ações afirmativas separados por ;
    /// </summary>
    public string? AcoesAfirmativas { get; private set; }

    /// <summary>
    /// Descrição de PCD (Pessoa com Deficiência)
    /// </summary>
    public string? DescricaoPcd { get; private set; }

    // Redes sociais
    public string? Instagram { get; private set; }
    public string? Facebook { get; private set; }
    public string? Linkedin { get; private set; }

    // Endereço (Value Object)
    public Endereco? Endereco { get; private set; }

    // Relacionamento 1:1
    public Candidato? Candidato { get; private set; }

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
