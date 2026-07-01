namespace TalentBridge.Application.DTOs.Candidato;

/// <summary>
/// Requisição para salvar resultado do teste Big Five
/// </summary>
public class BigFiveRequestDto
{
    /// <summary>
    /// Pontuação de Extroversão (0-100)
    /// </summary>
    public int Extroversao { get; set; }

    /// <summary>
    /// Pontuação de Amabilidade (0-100)
    /// </summary>
    public int Amabilidade { get; set; }

    /// <summary>
    /// Pontuação de Consciência/Autodisciplina (0-100)
    /// </summary>
    public int Consciencia { get; set; }

    /// <summary>
    /// Pontuação de Estabilidade Emocional (0-100)
    /// </summary>
    public int EstabilidadeEmocional { get; set; }

    /// <summary>
    /// Pontuação de Abertura à Experiência (0-100)
    /// </summary>
    public int AberturaExperiencia { get; set; }
}

/// <summary>
/// Resposta com dados do teste Big Five
/// </summary>
public class BigFiveResponseDto
{
    public int Extroversao { get; set; }
    public int Amabilidade { get; set; }
    public int Consciencia { get; set; }
    public int EstabilidadeEmocional { get; set; }
    public int AberturaExperiencia { get; set; }
    public DateTime? DataUltimoTeste { get; set; }
    public bool RealizouTeste { get; set; }
}
