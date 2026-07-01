namespace TalentBridge.Api.Configurations;

/// <summary>
/// Configurações para geração e validação de tokens JWT
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// Chave secreta para assinatura do token (mínimo 32 caracteres)
    /// </summary>
    public string Secret { get; set; } = string.Empty;

    /// <summary>
    /// Emissor do token (quem gerou)
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// Audiência do token (para quem foi gerado)
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Tempo de expiração do Access Token em minutos (padrão: 120 = 2 horas)
    /// </summary>
    public int AccessTokenExpirationMinutes { get; set; } = 120;

    /// <summary>
    /// Tempo de expiração do Refresh Token em dias (padrão: 5 dias)
    /// </summary>
    public int RefreshTokenExpirationDays { get; set; } = 5;
}
