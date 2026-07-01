using FluentValidation;
using TalentBridge.Application.DTOs.Vaga;

namespace TalentBridge.Application.Validators;

/// <summary>
/// Validador para criação/edição de vagas
/// </summary>
public class VagaUpsertValidator : AbstractValidator<VagaUpsertRequestDto>
{
    public VagaUpsertValidator()
    {
        RuleFor(x => x.Titulo)
            .NotEmpty().WithMessage("Título é obrigatório.")
            .MaximumLength(100).WithMessage("Título deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Cargo)
            .NotEmpty().WithMessage("Cargo é obrigatório.")
            .MaximumLength(100).WithMessage("Cargo deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Descricao)
            .NotEmpty().WithMessage("Descrição é obrigatória.")
            .MaximumLength(2000).WithMessage("Descrição deve ter no máximo 2000 caracteres.");

        RuleFor(x => x.Atividades)
            .NotEmpty().WithMessage("Atividades são obrigatórias.")
            .MaximumLength(1000).WithMessage("Atividades devem ter no máximo 1000 caracteres.");

        RuleFor(x => x.Beneficios)
            .NotEmpty().WithMessage("Benefícios são obrigatórios.")
            .MaximumLength(1000).WithMessage("Benefícios devem ter no máximo 1000 caracteres.");

        RuleFor(x => x.Salario)
            .GreaterThanOrEqualTo(0).WithMessage("Salário deve ser maior ou igual a zero.");

        RuleFor(x => x.Estado)
            .NotEmpty().WithMessage("Estado é obrigatório.")
            .Length(2).WithMessage("Estado deve ter 2 caracteres (UF).");

        RuleFor(x => x.Cidade)
            .NotEmpty().WithMessage("Cidade é obrigatória.")
            .MaximumLength(100).WithMessage("Cidade deve ter no máximo 100 caracteres.");

        RuleFor(x => x.DataCandidaturaInicio)
            .NotEmpty().WithMessage("Data de início é obrigatória.");

        RuleFor(x => x.DataCandidaturaFim)
            .NotEmpty().WithMessage("Data de fim é obrigatória.")
            .GreaterThan(x => x.DataCandidaturaInicio).WithMessage("Data de fim deve ser posterior à data de início.");

        RuleFor(x => x.QuantidadeVagas)
            .GreaterThan(0).WithMessage("Quantidade de vagas deve ser maior que zero.");

        RuleFor(x => x.LinkExterno)
            .MaximumLength(500).WithMessage("Link externo deve ter no máximo 500 caracteres.");
    }
}
