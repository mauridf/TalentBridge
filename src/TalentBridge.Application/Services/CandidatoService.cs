using FluentResults;
using FluentValidation;
using Mapster;
using Microsoft.Extensions.Logging;
using TalentBridge.Application.DTOs.Candidato;
using TalentBridge.Application.Interfaces;
using TalentBridge.Domain.Entities;
using TalentBridge.Domain.Enums;
using TalentBridge.Domain.Interfaces;

namespace TalentBridge.Application.Services;

/// <summary>
/// Implementação do serviço de candidatos
/// </summary>
public class CandidatoService : ICandidatoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;
    private readonly IValidator<CriarCandidatoRequestDto> _criarValidator;
    private readonly ILogger<CandidatoService> _logger;

    // ID do perfil Candidato (deve vir do banco, mas para simplificar usamos o GUID do seed)
    private static readonly Guid PerfilCandidatoId = Guid.Parse("a1b2c3d4-0004-4000-8000-000000000004");

    public CandidatoService(
        IUnitOfWork unitOfWork,
        IAuthService authService,
        IValidator<CriarCandidatoRequestDto> criarValidator,
        ILogger<CandidatoService> logger)
    {
        _unitOfWork = unitOfWork;
        _authService = authService;
        _criarValidator = criarValidator;
        _logger = logger;
    }

    /// <summary>
    /// Cria um novo candidato
    /// </summary>
    public async Task<Result<CriarCandidatoResponseDto>> CriarAsync(
        CriarCandidatoRequestDto request,
        CancellationToken cancellationToken = default)
    {
        // Validar request
        var validationResult = await _criarValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var erros = validationResult.Errors.Select(e => e.ErrorMessage);
            return Result.Fail<CriarCandidatoResponseDto>(erros);
        }

        // Verificar se email já existe
        var emailExiste = await _unitOfWork.Usuarios.EmailExisteAsync(request.Email, cancellationToken);
        if (emailExiste)
        {
            return Result.Fail<CriarCandidatoResponseDto>("EMAIL_JA_EXISTENTE");
        }

        // Hash da senha
        var senhaHash = _authService.HashSenha(request.Senha);

        // Buscar parceiro se código foi informado
        Guid? parceiroId = null;
        if (!string.IsNullOrWhiteSpace(request.CodigoParceiro))
        {
            var parceiro = await _unitOfWork.Parceiros
                .FindSingleAsync(p => p.CodigoPublico == request.CodigoParceiro.ToUpperInvariant(), cancellationToken: cancellationToken);
            parceiroId = parceiro?.Id;
        }

        // Criar entidade Candidato
        var candidato = new Candidato(
            nome: request.Nome,
            email: request.Email,
            senhaHash: senhaHash,
            perfilId: PerfilCandidatoId,
            dataNascimento: request.DataNascimento)
        {
            Telefone = request.Telefone,
            NomeSocial = request.NomeSocial,
            ParceiroId = parceiroId
        };

        await _unitOfWork.Candidatos.AddAsync(candidato, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Candidato criado: {Email} | ID: {Id}", request.Email, candidato.Id);

        // TODO: Enviar email de confirmação

        return Result.Ok(new CriarCandidatoResponseDto
        {
            Id = candidato.Id,
            Nome = candidato.Nome,
            Email = candidato.Email,
            Mensagem = "Candidato cadastrado com sucesso! Verifique seu email para confirmar a conta."
        });
    }

    /// <summary>
    /// Confirma o email do candidato
    /// </summary>
    public async Task<Result> ConfirmarEmailAsync(
        ConfirmarEmailRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var candidato = await _unitOfWork.Candidatos.GetByTokenConfirmacaoAsync(request.Token, cancellationToken);

        if (candidato == null || candidato.Email != request.Email.ToLowerInvariant().Trim())
        {
            return Result.Fail("TOKEN_INVALIDO");
        }

        candidato.ConfirmarEmail();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Email confirmado para: {Email}", request.Email);

        return Result.Ok();
    }

    /// <summary>
    /// Reenvia email de confirmação
    /// </summary>
    public async Task<Result> ReenviarConfirmacaoEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var candidato = await _unitOfWork.Candidatos.GetByEmailAsync(email, cancellationToken);

        if (candidato == null || candidato.Status == StatusUsuarioEnum.Ativo)
        {
            // Não revelar se o email existe
            return Result.Ok();
        }

        candidato.GerarNovoTokenConfirmacao();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // TODO: Reenviar email de confirmação

        return Result.Ok();
    }

    /// <summary>
    /// Busca candidato por ID
    /// </summary>
    public async Task<Result<CandidatoResponseDto>> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var candidato = await _unitOfWork.Candidatos.GetByIdCompletoAsync(id, cancellationToken);

        if (candidato == null)
        {
            return Result.Fail<CandidatoResponseDto>("CANDIDATO_NAO_ENCONTRADO");
        }

        var dto = candidato.Adapt<CandidatoResponseDto>();
        dto.Status = candidato.Status.ToString();
        dto.RealizouBigFive = candidato.RealizouTesteBigFive();

        return Result.Ok(dto);
    }

    /// <summary>
    /// Busca candidato por email
    /// </summary>
    public async Task<Result<CandidatoResponseDto>> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var candidato = await _unitOfWork.Candidatos.GetByEmailAsync(email, cancellationToken);

        if (candidato == null)
        {
            return Result.Fail<CandidatoResponseDto>("CANDIDATO_NAO_ENCONTRADO");
        }

        var dto = candidato.Adapt<CandidatoResponseDto>();
        dto.Status = candidato.Status.ToString();

        return Result.Ok(dto);
    }

    /// <summary>
    /// Edita dados do candidato
    /// </summary>
    public async Task<Result<CandidatoResponseDto>> EditarAsync(
        Guid id,
        EditarCandidatoRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var candidato = await _unitOfWork.Candidatos.GetByIdAsync(id, cancellationToken);

        if (candidato == null)
        {
            return Result.Fail<CandidatoResponseDto>("CANDIDATO_NAO_ENCONTRADO");
        }

        candidato.AtualizarDados(
            nome: request.Nome ?? candidato.Nome,
            telefone: request.Telefone ?? candidato.Telefone);

        if (request.NomeSocial != null || request.DataNascimento.HasValue)
        {
            candidato.AtualizarDadosCandidato(
                nomeSocial: request.NomeSocial ?? candidato.NomeSocial,
                dataNascimento: request.DataNascimento ?? candidato.DataNascimento);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = candidato.Adapt<CandidatoResponseDto>();
        dto.Status = candidato.Status.ToString();

        return Result.Ok(dto);
    }
}
