namespace TalentBridge.Application.DTOs.Usuario;

/// <summary>
/// Requisição para criar um recrutador via convite
/// </summary>
public class CriarRecrutadorRequestDto
{
    public string TokenConvite { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string ConfirmacaoSenha { get; set; } = string.Empty;
}

/// <summary>
/// Requisição para criar um recrutador diretamente (gestor logado)
/// </summary>
public class CriarRecrutadorDiretoRequestDto
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}

/// <summary>
/// Resposta após criar um recrutador
/// </summary>
public class CriarRecrutadorResponseDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string EmpresaNome { get; set; } = string.Empty;
    public string Mensagem { get; set; } = string.Empty;
}

/// <summary>
/// DTO para listagem de recrutadores
/// </summary>
public class RecrutadorListaDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
