namespace TalentBridge.Application.DTOs.Usuario;

/// <summary>
/// Requisição de login
/// </summary>
public class LoginRequestDto
{
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}

/// <summary>
/// Resposta do login (quando não há multi-perfil/empresa)
/// </summary>
public class LoginResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Perfil { get; set; } = string.Empty;
    public Guid? EmpresaId { get; set; }
    public string? EmpresaNome { get; set; }
}

/// <summary>
/// Resposta quando há múltiplos perfis disponíveis
/// </summary>
public class LoginMultiPerfilResponseDto
{
    public bool MultiPerfil { get; set; }
    public bool MultiEmpresa { get; set; }
    public List<PerfilDisponivelDto>? PerfisDisponiveis { get; set; }
    public List<EmpresaDisponivelDto>? Empresas { get; set; }
    public string TokenTemporario { get; set; } = string.Empty;
}

public class PerfilDisponivelDto
{
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
}

public class EmpresaDisponivelDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public string PerfilCodigo { get; set; } = string.Empty;
}

/// <summary>
/// Requisição para selecionar perfil
/// </summary>
public class SelecionarPerfilRequestDto
{
    public string TokenTemporario { get; set; } = string.Empty;
    public string PerfilCodigo { get; set; } = string.Empty;
}

/// <summary>
/// Requisição para selecionar empresa
/// </summary>
public class SelecionarEmpresaRequestDto
{
    public Guid IdEmpresa { get; set; }
}

/// <summary>
/// Resposta do refresh token
/// </summary>
public class RefreshTokenResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
}

/// <summary>
/// Requisição para verificar disponibilidade de email
/// </summary>
public class EmailDisponivelRequestDto
{
    public string Email { get; set; } = string.Empty;
}

/// <summary>
/// Requisição de recuperação de senha
/// </summary>
public class RecuperacaoSenhaRequestDto
{
    public string Token { get; set; } = string.Empty;
    public string NovaSenha { get; set; } = string.Empty;
}

/// <summary>
/// Requisição para enviar email de recuperação
/// </summary>
public class ResetSenhaEmailRequestDto
{
    public string Email { get; set; } = string.Empty;
}
