using FluentValidation;
using TalentBridge.Application.DTOs.Empresa;

namespace TalentBridge.Application.Validators;

/// <summary>
/// Validador para criação de empresa
/// </summary>
public class CriarEmpresaValidator : AbstractValidator<CriarEmpresaRequestDto>
{
    public CriarEmpresaValidator()
    {
        RuleFor(x => x.NomeGestor)
            .NotEmpty().WithMessage("Nome do gestor é obrigatório.")
            .MinimumLength(3).WithMessage("Nome deve ter no mínimo 3 caracteres.")
            .MaximumLength(128).WithMessage("Nome deve ter no máximo 128 caracteres.");

        RuleFor(x => x.EmailGestor)
            .NotEmpty().WithMessage("Email é obrigatório.")
            .EmailAddress().WithMessage("Email inválido.");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("Senha é obrigatória.")
            .MinimumLength(8).WithMessage("Senha deve ter no mínimo 8 caracteres.");

        RuleFor(x => x.ConfirmacaoSenha)
            .Equal(x => x.Senha).WithMessage("Senhas não conferem.");

        RuleFor(x => x.NomeEmpresa)
            .NotEmpty().WithMessage("Nome da empresa é obrigatório.")
            .MaximumLength(200).WithMessage("Nome da empresa deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Cnpj)
            .NotEmpty().WithMessage("CNPJ é obrigatório.")
            .Length(14).WithMessage("CNPJ deve ter 14 dígitos.")
            .Matches(@"^\d{14}$").WithMessage("CNPJ deve conter apenas números.");

        RuleFor(x => x.TelefoneEmpresa)
            .NotEmpty().WithMessage("Telefone é obrigatório.")
            .MaximumLength(20).WithMessage("Telefone deve ter no máximo 20 caracteres.");

        RuleFor(x => x.SegmentoId)
            .NotEmpty().WithMessage("Segmento é obrigatório.");

        RuleFor(x => x.TokenConvite)
            .NotEmpty().WithMessage("Token de convite é obrigatório.");
    }
}
