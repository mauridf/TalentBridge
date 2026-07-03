using FluentResults;
using FluentValidation;
using Mapster;
using Microsoft.Extensions.Logging;
using TalentBridge.Application.DTOs.Empresa;
using TalentBridge.Application.DTOs.Usuario;
using TalentBridge.Application.Interfaces;
using TalentBridge.Domain.Entities;
using TalentBridge.Domain.Enums;
using TalentBridge.Domain.Interfaces;

namespace TalentBridge.Application.Services;

/// <summary>
/// Implementação do serviço de empresas
/// </summary>
public class EmpresaService : IEmpresaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;
    private readonly IValidator<CriarEmpresaRequestDto> _criarValidator;
    private readonly ILogger<EmpresaService> _logger;

    private static readonly Guid PerfilGestorId = Guid.Parse("a1b2c3d4-0002-4000-8000-000000000002");
    private static readonly Guid PerfilRecrutadorId = Guid.Parse("a1b2c3d4-0003-4000-8000-000000000003");

    public EmpresaService(
        IUnitOfWork unitOfWork,
        IAuthService authService,
        IValidator<CriarEmpresaRequestDto> criarValidator,
        ILogger<EmpresaService> logger)
    {
        _unitOfWork = unitOfWork;
        _authService = authService;
        _criarValidator = criarValidator;
        _logger = logger;
    }

    /// <summary>
    /// Cria uma empresa com gestor via convite
    /// </summary>
    public async Task<Result<CriarEmpresaResponseDto>> CriarEmpresaComGestorAsync(
        CriarEmpresaRequestDto request,
        CancellationToken cancellationToken = default)
    {
        // Validar
        var validationResult = await _criarValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Result.Fail<CriarEmpresaResponseDto>(
                validationResult.Errors.Select(e => e.ErrorMessage));
        }

        // Se tiver token, validar convite
        Convite? convite = null;
        if (request.TokenConvite.HasValue)
        {
            convite = await _unitOfWork.Convites
                .FindSingleAsync(c => c.Token == request.TokenConvite.Value, cancellationToken: cancellationToken);

            if (convite == null || !convite.EstaValido())
            {
                return Result.Fail<CriarEmpresaResponseDto>("CONVITE_INVALIDO");
            }
        }

        // Verificar se email já existe
        if (await _unitOfWork.Usuarios.EmailExisteAsync(request.EmailGestor, cancellationToken))
        {
            return Result.Fail<CriarEmpresaResponseDto>("EMAIL_JA_EXISTENTE");
        }

        // Verificar se CNPJ já existe
        var cnpjExiste = await _unitOfWork.Empresas.GetByCnpjAsync(request.Cnpj, cancellationToken);
        if (cnpjExiste != null)
        {
            return Result.Fail<CriarEmpresaResponseDto>("CNPJ_JA_CADASTRADO");
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            // Criar empresa
            var empresa = new Empresa(
                nome: request.NomeEmpresa,
                cnpj: request.Cnpj,
                email: request.EmailGestor,
                telefone: request.TelefoneEmpresa,
                segmentoId: request.SegmentoId);

            await _unitOfWork.Empresas.AddAsync(empresa, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Criar gestor
            var senhaHash = _authService.HashSenha(request.Senha);
            var gestor = new Gestor(
                nome: request.NomeGestor,
                email: request.EmailGestor,
                senhaHash: senhaHash,
                perfilId: PerfilGestorId,
                empresaId: empresa.Id);

            gestor.Ativar();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Vincular usuário à empresa
            var usuarioEmpresa = new UsuarioEmpresa(gestor.Id, empresa.Id, PerfilGestorId);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Aceitar convite (se houver)
            if (convite != null)
            {
                convite.Aceitar();
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            _logger.LogInformation(
                convite != null
                    ? "Empresa criada via convite: {Empresa} | Gestor: {Email}"
                    : "Empresa criada (auto-cadastro): {Empresa} | Gestor: {Email}",
                empresa.Nome, request.EmailGestor);

            return Result.Ok(new CriarEmpresaResponseDto
            {
                EmpresaId = empresa.Id,
                GestorId = gestor.Id,
                NomeEmpresa = empresa.Nome,
                NomeGestor = gestor.Nome,
                Mensagem = "Empresa cadastrada com sucesso!"
            });
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            _logger.LogError(ex, "Erro ao criar empresa: {Email}", request.EmailGestor);
            throw;
        }
    }

    /// <summary>
    /// Cria um recrutador via convite
    /// </summary>
    public async Task<Result<ConviteResponseDto>> CriarRecrutadorAsync(
        Guid tokenConvite,
        string nome,
        string email,
        string senha,
        string? nomeSocial = null,
        CancellationToken cancellationToken = default)
    {
        var convite = await _unitOfWork.Convites
            .FindSingleAsync(c => c.Token == tokenConvite, cancellationToken: cancellationToken);

        if (convite == null || !convite.EstaValido() || convite.Tipo != TipoConviteEnum.Recrutador)
        {
            return Result.Fail<ConviteResponseDto>("CONVITE_INVALIDO");
        }

        if (await _unitOfWork.Usuarios.EmailExisteAsync(email, cancellationToken))
        {
            return Result.Fail<ConviteResponseDto>("EMAIL_JA_EXISTENTE");
        }

        var senhaHash = _authService.HashSenha(senha);

        var recrutador = new Recrutador(
            nome: nome,
            email: email,
            senhaHash: senhaHash,
            perfilId: PerfilRecrutadorId,
            empresaId: convite.EmpresaResponsavelId,
            conviteId: convite.Id,
            nomeSocial: nomeSocial);

        recrutador.Ativar();
        await _unitOfWork.Recrutadores.AddAsync(recrutador, cancellationToken);

        convite.Aceitar();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Recrutador criado: {Email} | Empresa: {EmpresaId}", email, convite.EmpresaResponsavelId);

        return Result.Ok(convite.Adapt<ConviteResponseDto>());
    }

    /// <summary>
    /// Cria um convite para empresa ou recrutador
    /// </summary>
    public async Task<Result<ConviteResponseDto>> CriarConviteAsync(
        Guid empresaId,
        CriarConviteRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var tipo = request.Tipo == 0 ? TipoConviteEnum.Recrutador : TipoConviteEnum.Empresa;

        var convite = new Convite(
            email: request.Email,
            tipo: tipo,
            empresaResponsavelId: empresaId,
            diasExpiracao: 7,
            cnpj: request.Cnpj,
            nomeEmpresa: request.NomeEmpresa,
            nomeResponsavel: request.NomeResponsavel,
            telefone: request.Telefone);

        // Atualizar campos opcionais
        // (Usamos reflexão pois os campos são privados no construtor)
        typeof(Convite).GetProperty("Cnpj")?.SetValue(convite, request.Cnpj);
        typeof(Convite).GetProperty("NomeEmpresa")?.SetValue(convite, request.NomeEmpresa);
        typeof(Convite).GetProperty("NomeResponsavel")?.SetValue(convite, request.NomeResponsavel);
        typeof(Convite).GetProperty("Telefone")?.SetValue(convite, request.Telefone);

        await _unitOfWork.Convites.AddAsync(convite, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Convite criado: {Email} | Tipo: {Tipo}", request.Email, tipo);

        // TODO: Enviar email com link de convite

        return Result.Ok(convite.Adapt<ConviteResponseDto>());
    }

    /// <summary>
    /// Valida um token de convite
    /// </summary>
    public async Task<Result<ConviteResponseDto>> ValidarConviteAsync(
        Guid token,
        CancellationToken cancellationToken = default)
    {
        var convite = await _unitOfWork.Convites
            .FindSingleAsync(c => c.Token == token, cancellationToken: cancellationToken);

        if (convite == null || !convite.EstaValido())
        {
            return Result.Fail<ConviteResponseDto>("CONVITE_INVALIDO");
        }

        return Result.Ok(convite.Adapt<ConviteResponseDto>());
    }

    /// <summary>
    /// Lista convites de uma empresa
    /// </summary>
    public async Task<Result<IEnumerable<ConviteResponseDto>>> ListarConvitesAsync(
        Guid empresaId,
        CancellationToken cancellationToken = default)
    {
        var convites = await _unitOfWork.Convites
            .FindAsync(c => c.EmpresaResponsavelId == empresaId, cancellationToken: cancellationToken);

        return Result.Ok(convites.Adapt<IEnumerable<ConviteResponseDto>>());
    }
}
