namespace TalentBridge.Application.DTOs.Common;

/// <summary>
/// Padrão de resposta da API
/// </summary>
public class ResultadoDto<T>
{
    public bool Sucesso { get; set; }
    public T? Valor { get; set; }
    public List<ErroDto>? Erros { get; set; }

    public static ResultadoDto<T> Ok(T valor) => new()
    {
        Sucesso = true,
        Valor = valor
    };

    public static ResultadoDto<T> Falha(string codigo, string mensagem) => new()
    {
        Sucesso = false,
        Erros = new List<ErroDto> { new() { Codigo = codigo, Mensagem = mensagem } }
    };

    public static ResultadoDto<T> Falha(List<ErroDto> erros) => new()
    {
        Sucesso = false,
        Erros = erros
    };
}

public class ErroDto
{
    public string Codigo { get; set; } = string.Empty;
    public string Mensagem { get; set; } = string.Empty;
}
