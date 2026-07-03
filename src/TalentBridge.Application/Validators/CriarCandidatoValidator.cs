using FluentValidation;
using TalentBridge.Application.DTOs.Candidato;

namespace TalentBridge.Application.Validators;

/// <summary>
/// Validador para criação de candidato
/// </summary>
public class CriarCandidatoValidator : AbstractValidator<CriarCandidatoRequestDto>
{
    public CriarCandidatoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MinimumLength(3).WithMessage("Nome deve ter no mínimo 3 caracteres.")
            .MaximumLength(128).WithMessage("Nome deve ter no máximo 128 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório.")
            .EmailAddress().WithMessage("Email inválido.")
            .MaximumLength(100).WithMessage("Email deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("Senha é obrigatória.")
            .MinimumLength(8).WithMessage("Senha deve ter no mínimo 8 caracteres.")
            .MaximumLength(50).WithMessage("Senha deve ter no máximo 50 caracteres.")
            .Matches(@"[A-Z]").WithMessage("Senha deve conter pelo menos uma letra maiúscula.")
            .Matches(@"[a-z]").WithMessage("Senha deve conter pelo menos uma letra minúscula.")
            .Matches(@"[0-9]").WithMessage("Senha deve conter pelo menos um número.")
            .Matches(@"[!@#$%^&*(),.?"":{}|<>]").WithMessage("Senha deve conter pelo menos um caractere especial.");

        RuleFor(x => x.ConfirmacaoSenha)
            .Equal(x => x.Senha).WithMessage("Senhas não conferem.");

        RuleFor(x => x.DataNascimento)
            .NotEmpty().WithMessage("Data de nascimento é obrigatória.")
            .Must(data => data <= DateTime.UtcNow.AddYears(-14)).WithMessage("Candidato deve ter pelo menos 14 anos.")
            .Must(data => data >= DateTime.UtcNow.AddYears(-120)).WithMessage("Data de nascimento inválida.");

        RuleFor(x => x.Telefone)
            .MaximumLength(20).WithMessage("Telefone deve ter no máximo 20 caracteres.");

        RuleFor(x => x.NomeSocial)
            .MaximumLength(100).WithMessage("Nome social deve ter no máximo 100 caracteres.");
    }
}
