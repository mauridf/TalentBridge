using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TalentBridge.Api.Configurations;
using TalentBridge.Application.Interfaces;
using TalentBridge.Domain.Entities;

namespace TalentBridge.Infrastructure.Services;

/// <summary>
/// Implementação do serviço de tokens JWT
/// </summary>
public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;

    public TokenService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    /// <summary>
    /// Gera um access token JWT com claims do usuário
    /// </summary>
    public string GerarAccessToken(Usuario usuario, string perfilCodigo, string? empresaId = null, string? empresaNome = null)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new("id", usuario.Id.ToString()),
            new("perfil", perfilCodigo),
            new("nome", usuario.Nome),
            new("email", usuario.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        if (!string.IsNullOrWhiteSpace(empresaId))
        {
            claims.Add(new Claim("idEmpresa", empresaId));
        }

        if (!string.IsNullOrWhiteSpace(empresaNome))
        {
            claims.Add(new Claim("empresaNome", empresaNome));
        }

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// Gera um refresh token aleatório e seguro
    /// </summary>
    public string GerarRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    /// <summary>
    /// Gera um token temporário para o fluxo de seleção de perfil
    /// </summary>
    public string GerarTokenTemporario(Usuario usuario)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new("id", usuario.Id.ToString()),
            new("nome", usuario.Nome),
            new("email", usuario.Email),
            new("tipo", "temporario"),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(5), // Token temporário expira em 5 minutos
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// Valida um token JWT e retorna as claims
    /// </summary>
    public ClaimsPrincipal? ValidarToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtSettings.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(1) // Margem de 1 minuto
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            return principal;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Extrai o ID do usuário de um token JWT
    /// </summary>
    public Guid? ExtrairUsuarioId(string token)
    {
        var principal = ValidarToken(token);
        var idClaim = principal?.FindFirst("id")?.Value;
        return Guid.TryParse(idClaim, out var id) ? id : null;
    }

    /// <summary>
    /// Gera um token para convite de recrutador/empresa
    /// </summary>
    public string GerarTokenConvite(Convite convite)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new("conviteId", convite.Id.ToString()),
            new("tipoConvite", convite.Tipo.ToString()),
            new("email", convite.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: convite.DataExpiracao,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
