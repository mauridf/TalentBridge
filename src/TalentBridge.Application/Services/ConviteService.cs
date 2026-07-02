using Microsoft.Extensions.Logging;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.DTOs.Usuario;
using TalentBridge.Application.Interfaces;
using TalentBridge.Domain.Entities;
using TalentBridge.Domain.Enums;
using TalentBridge.Domain.Interfaces;

namespace TalentBridge.Application.Services;

public class ConviteService : IConviteService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ConviteService> _logger;

    public ConviteService(IUnitOfWork unitOfWork, ILogger<ConviteService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ResultadoDto<ConviteResponseDto>> CriarConvite(CriarConviteRequestDto request, Guid empresaResponsavelId)
    {
        _logger.LogInformation("Criando convite para {Email} pela empresa {EmpresaId}", request.Email, empresaResponsavelId);

        var empresa = await _unitOfWork.Empresas.GetByIdAsync(empresaResponsavelId);
        if (empresa == null)
            return ResultadoDto<ConviteResponseDto>.Falha("EMPRESA_NAO_ENCONTRADA", "Empresa responsável não encontrada.");

        var existeConvite = await _unitOfWork.Convites.ExistsAsync(c => c.Email == request.Email && c.Status == StatusConviteEnum.Pendente);
        if (existeConvite)
            return ResultadoDto<ConviteResponseDto>.Falha("CONVITE_EXISTENTE", "Já existe um convite pendente para este email.");

        var tipo = (TipoConviteEnum)request.Tipo;
        var convite = new Convite(request.Email, tipo, empresaResponsavelId, 7, request.Cnpj, request.NomeEmpresa, request.NomeResponsavel, request.Telefone);

        await _unitOfWork.Convites.AddAsync(convite);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Convite {ConviteId} criado para {Email}", convite.Id, request.Email);

        return ResultadoDto<ConviteResponseDto>.Ok(new ConviteResponseDto
        {
            Id = convite.Id,
            Email = convite.Email,
            Tipo = convite.Tipo.ToString(),
            Status = convite.Status.ToString(),
            Token = convite.Token,
            DataExpiracao = convite.DataExpiracao,
            DataAceite = convite.DataAceite
        });
    }

    public async Task<ResultadoDto<ConviteResponseDto>> ValidarToken(string token)
    {
        _logger.LogInformation("Validando token de convite: {Token}", token);

        if (!Guid.TryParse(token, out var tokenGuid))
            return ResultadoDto<ConviteResponseDto>.Falha("TOKEN_INVALIDO", "Token inválido.");

        var convite = await _unitOfWork.Convites.FindSingleAsync(c => c.Token == tokenGuid);
        if (convite == null)
            return ResultadoDto<ConviteResponseDto>.Falha("CONVITE_NAO_ENCONTRADO", "Convite não encontrado para este token.");

        if (!convite.EstaValido())
            return ResultadoDto<ConviteResponseDto>.Falha("CONVITE_INVALIDO", "Convite expirado ou já utilizado.");

        return ResultadoDto<ConviteResponseDto>.Ok(new ConviteResponseDto
        {
            Id = convite.Id,
            Email = convite.Email,
            Tipo = convite.Tipo.ToString(),
            Status = convite.Status.ToString(),
            Token = convite.Token,
            DataExpiracao = convite.DataExpiracao,
            DataAceite = convite.DataAceite
        });
    }

    public async Task<ResultadoDto<List<ConviteResponseDto>>> ListarPorEmpresa(Guid empresaId)
    {
        _logger.LogInformation("Listando convites da empresa {EmpresaId}", empresaId);

        var convites = await _unitOfWork.Convites.FindAsync(c => c.EmpresaResponsavelId == empresaId);

        var result = convites.Select(c => new ConviteResponseDto
        {
            Id = c.Id,
            Email = c.Email,
            Tipo = c.Tipo.ToString(),
            Status = c.Status.ToString(),
            Token = c.Token,
            DataExpiracao = c.DataExpiracao,
            DataAceite = c.DataAceite
        }).ToList();

        return ResultadoDto<List<ConviteResponseDto>>.Ok(result);
    }

    public async Task<ResultadoDto<bool>> Inativar(Guid conviteId)
    {
        _logger.LogInformation("Inativando convite {ConviteId}", conviteId);

        var convite = await _unitOfWork.Convites.GetByIdAsync(conviteId);
        if (convite == null)
            return ResultadoDto<bool>.Falha("CONVITE_NAO_ENCONTRADO", "Convite não encontrado.");

        convite.Inativar();
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Convite {ConviteId} inativado", conviteId);

        return ResultadoDto<bool>.Ok(true);
    }
}
