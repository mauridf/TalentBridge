namespace TalentBridge.Application.DTOs.Candidato;

/// <summary>
/// Requisição para criar/editar perfil pessoal (upsert)
/// </summary>
public class PerfilPessoalUpsertRequestDto
{
    /// <summary>
    /// Cor/Raça (código do domínio)
    /// </summary>
    public int? CorRaca { get; set; }

    /// <summary>
    /// Pronome (código do domínio)
    /// </summary>
    public int? Pronome { get; set; }

    /// <summary>
    /// Identidade de gênero (código do domínio)
    /// </summary>
    public int? IdentidadeGenero { get; set; }

    /// <summary>
    /// Orientação sexual (código do domínio)
    /// </summary>
    public int? OrientacaoSexual { get; set; }

    /// <summary>
    /// CPF (apenas números)
    /// </summary>
    public string? Cpf { get; set; }

    /// <summary>
    /// RG
    /// </summary>
    public string? Rg { get; set; }

    /// <summary>
    /// Texto "Sobre mim"
    /// </summary>
    public string? SobreMim { get; set; }

    /// <summary>
    /// Local de residência
    /// </summary>
    public string? LocalResidencia { get; set; }

    /// <summary>
    /// Ações afirmativas (códigos separados por ;)
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

    // Endereço
    public string? CEP { get; set; }
    public string? Rua { get; set; }
    public string? Numero { get; set; }
    public string? Bairro { get; set; }
    public string? Cidade { get; set; }
    public string? Estado { get; set; }
    public string? Complemento { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}

/// <summary>
/// Resposta com dados do perfil pessoal
/// </summary>
public class PerfilPessoalResponseDto
{
    public Guid Id { get; set; }
    public int? CorRaca { get; set; }
    public int? Pronome { get; set; }
    public int? IdentidadeGenero { get; set; }
    public int? OrientacaoSexual { get; set; }
    public string? Cpf { get; set; }
    public string? Rg { get; set; }
    public string? SobreMim { get; set; }
    public string? LocalResidencia { get; set; }
    public string? AcoesAfirmativas { get; set; }
    public string? DescricaoPcd { get; set; }
    public string? Instagram { get; set; }
    public string? Facebook { get; set; }
    public string? Linkedin { get; set; }
    public EnderecoResponseDto? Endereco { get; set; }
}

/// <summary>
/// Endereço na resposta
/// </summary>
public class EnderecoResponseDto
{
    public string? CEP { get; set; }
    public string? Rua { get; set; }
    public string? Numero { get; set; }
    public string? Bairro { get; set; }
    public string? Cidade { get; set; }
    public string? Estado { get; set; }
    public string? Complemento { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}
