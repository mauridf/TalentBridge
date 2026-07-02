namespace TalentBridge.Domain.Entities;

public class ApiLog : BaseEntity
{
    public string Metodo { get; private set; } = string.Empty;
    public string Rota { get; private set; } = string.Empty;
    public int StatusCode { get; private set; }
    public long DuracaoMs { get; private set; }
    public string? Requisicao { get; private set; }
    public string? Resposta { get; private set; }
    public string? Ip { get; private set; }
    public string? UserAgent { get; private set; }
    public Guid? UsuarioId { get; private set; }

    protected ApiLog() { }

    public ApiLog(string metodo, string rota, int statusCode, long duracaoMs, string? requisicao = null, string? resposta = null, string? ip = null, string? userAgent = null, Guid? usuarioId = null)
    {
        Metodo = metodo.ToUpperInvariant();
        Rota = rota;
        StatusCode = statusCode;
        DuracaoMs = duracaoMs;
        Requisicao = requisicao;
        Resposta = resposta;
        Ip = ip;
        UserAgent = userAgent;
        UsuarioId = usuarioId;
    }
}
