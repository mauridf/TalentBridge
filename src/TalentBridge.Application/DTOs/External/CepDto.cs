namespace TalentBridge.Application.DTOs.External;

/// <summary>
/// Resposta da consulta de CEP
/// </summary>
public class CepResponseDto
{
    public string Cep { get; set; } = string.Empty;
    public string Logradouro { get; set; } = string.Empty;
    public string? Complemento { get; set; }
    public string Bairro { get; set; } = string.Empty;
    public string Localidade { get; set; } = string.Empty;
    public string Uf { get; set; } = string.Empty;
    public string? Ibge { get; set; }
}

/// <summary>
/// Estado brasileiro
/// </summary>
public class EstadoDto
{
    public string Sigla { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
}

/// <summary>
/// Município
/// </summary>
public class MunicipioDto
{
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
}
