namespace TalentBridge.Application.DTOs.Candidato;

/// <summary>
/// Requisição para criar um novo candidato
/// </summary>
public class CriarCandidatoRequestDto
{
    /// <summary>
    /// Nome completo do candidato
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Email do candidato (será usado como login)
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Senha (mínimo 8 caracteres)
    /// </summary>
    public string Senha { get; set; } = string.Empty;

    /// <summary>
    /// Confirmação de senha
    /// </summary>
    public string ConfirmacaoSenha { get; set; } = string.Empty;

    /// <summary>
    /// Data de nascimento
    /// </summary>
    public DateTime DataNascimento { get; set; }

    /// <summary>
    /// Telefone (opcional)
    /// </summary>
    public string? Telefone { get; set; }

    /// <summary>
    /// Nome social (opcional)
    /// </summary>
    public string? NomeSocial { get; set; }

    /// <summary>
    /// Código público do parceiro que indicou (opcional)
    /// </summary>
    public string? CodigoParceiro { get; set; }
}

/// <summary>
/// Resposta após criar um candidato
/// </summary>
public class CriarCandidatoResponseDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Mensagem { get; set; } = string.Empty;
}

/// <summary>
/// Requisição para editar dados do candidato
/// </summary>
public class EditarCandidatoRequestDto
{
    public string? Nome { get; set; }
    public string? NomeSocial { get; set; }
    public string? Telefone { get; set; }
    public DateTime? DataNascimento { get; set; }
}

/// <summary>
/// Requisição para confirmação de email
/// </summary>
public class ConfirmarEmailRequestDto
{
    public string Email { get; set; } = string.Empty;
    public Guid Token { get; set; }
}

/// <summary>
/// Requisição para reenviar email de confirmação
/// </summary>
public class ReenviarConfirmacaoEmailRequestDto
{
    public string Email { get; set; } = string.Empty;
}

/// <summary>
/// Dados do candidato para resposta
/// </summary>
public class CandidatoResponseDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? NomeSocial { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public DateTime DataNascimento { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool RealizouBigFive { get; set; }
    public DateTime? DataUltimoTesteBigFive { get; set; }
    public DateTime CreatedAt { get; set; }
}
