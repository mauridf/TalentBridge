using Mapster;
using Microsoft.Extensions.Logging;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.DTOs.Usuario;
using TalentBridge.Application.Interfaces;
using TalentBridge.Domain.Entities;
using TalentBridge.Domain.Enums;
using TalentBridge.Domain.Interfaces;

namespace TalentBridge.Application.Services;

public class RecrutadorService : IRecrutadorService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;
    private readonly ILogger<RecrutadorService> _logger;

    private static readonly Guid PerfilRecrutadorId = Guid.Parse("a1b2c3d4-0003-4000-8000-000000000003");

    public RecrutadorService(
        IUnitOfWork unitOfWork,
        IAuthService authService,
        ILogger<RecrutadorService> logger)
    {
        _unitOfWork = unitOfWork;
        _authService = authService;
        _logger = logger;
    }

    public async Task<ResultadoDto<CriarRecrutadorResponseDto>> CriarAsync(CriarRecrutadorRequestDto request)
    {
        // Validar se email já existe
        if (await _unitOfWork.Usuarios.EmailExisteAsync(request.Email))
        {
            return ResultadoDto<CriarRecrutadorResponseDto>.Falha("EMAIL_JA_EXISTENTE", "Este email já está cadastrado.");
        }

        Convite? convite = null;
        Guid empresaId;

        if (!string.IsNullOrWhiteSpace(request.TokenConvite))
        {
            // Fluxo via convite
            if (!Guid.TryParse(request.TokenConvite, out var tokenGuid))
            {
                return ResultadoDto<CriarRecrutadorResponseDto>.Falha("TOKEN_INVALIDO", "Token de convite inválido.");
            }

            convite = await _unitOfWork.Convites.FindSingleAsync(c => c.Token == tokenGuid);

            if (convite == null || !convite.EstaValido())
            {
                return ResultadoDto<CriarRecrutadorResponseDto>.Falha("CONVITE_INVALIDO", "Convite inválido ou expirado.");
            }

            if (convite.Tipo != TipoConviteEnum.Recrutador)
            {
                return ResultadoDto<CriarRecrutadorResponseDto>.Falha("CONVITE_TIPO_INVALIDO", "Este convite não é para recrutador.");
            }

            empresaId = convite.EmpresaResponsavelId;
        }
        else
        {
            return ResultadoDto<CriarRecrutadorResponseDto>.Falha("TOKEN_OBRIGATORIO", "Token de convite é obrigatório para este endpoint.");
        }

        // Buscar nome da empresa
        var empresa = await _unitOfWork.Empresas.GetByIdAsync(empresaId);
        var empresaNome = empresa?.Nome ?? "";

        // Criar recrutador
        var senhaHash = _authService.HashSenha(request.Senha);
        var recrutador = new Recrutador(
            nome: request.Nome,
            email: request.Email,
            senhaHash: senhaHash,
            perfilId: PerfilRecrutadorId,
            empresaId: empresaId,
            conviteId: convite!.Id);

        recrutador.Ativar();
        await _unitOfWork.Recrutadores.AddAsync(recrutador);

        if (convite != null)
        {
            convite.Aceitar();
        }

        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Recrutador criado via convite: {Email} | Empresa: {EmpresaId}", request.Email, empresaId);

        return ResultadoDto<CriarRecrutadorResponseDto>.Ok(new CriarRecrutadorResponseDto
        {
            Id = recrutador.Id,
            Nome = recrutador.Nome,
            Email = recrutador.Email,
            EmpresaNome = empresaNome,
            Mensagem = "Conta de recrutador criada com sucesso!"
        });
    }

    public async Task<ResultadoDto<CriarRecrutadorResponseDto>> CriarDiretoAsync(
        CriarRecrutadorDiretoRequestDto request, Guid empresaId)
    {
        if (await _unitOfWork.Usuarios.EmailExisteAsync(request.Email))
        {
            return ResultadoDto<CriarRecrutadorResponseDto>.Falha(
                "EMAIL_JA_EXISTENTE", "Este email já está cadastrado.");
        }

        var empresa = await _unitOfWork.Empresas.GetByIdAsync(empresaId);
        if (empresa == null)
        {
            return ResultadoDto<CriarRecrutadorResponseDto>.Falha(
                "EMPRESA_NAO_ENCONTRADA", "Empresa não encontrada.");
        }

        var senhaHash = _authService.HashSenha(request.Senha);
        var recrutador = new Recrutador(
            nome: request.Nome,
            email: request.Email,
            senhaHash: senhaHash,
            perfilId: PerfilRecrutadorId,
            empresaId: empresaId);

        recrutador.Ativar();
        await _unitOfWork.Recrutadores.AddAsync(recrutador);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Recrutador criado diretamente: {Email} | Empresa: {EmpresaId}",
            request.Email, empresaId);

        return ResultadoDto<CriarRecrutadorResponseDto>.Ok(new CriarRecrutadorResponseDto
        {
            Id = recrutador.Id,
            Nome = recrutador.Nome,
            Email = recrutador.Email,
            EmpresaNome = empresa.Nome,
            Mensagem = "Recrutador adicionado com sucesso!"
        });
    }
}
