using FluentResults;
using FluentValidation;
using Mapster;
using Microsoft.Extensions.Logging;
using TalentBridge.Application.DTOs.Common;
using TalentBridge.Application.DTOs.Vaga;
using TalentBridge.Application.Interfaces;
using TalentBridge.Domain.Entities;
using TalentBridge.Domain.Enums;
using TalentBridge.Domain.Interfaces;

namespace TalentBridge.Application.Services;

/// <summary>
/// Implementação do serviço de vagas
/// </summary>
public class VagaService : IVagaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<VagaUpsertRequestDto> _validator;
    private readonly ILogger<VagaService> _logger;

    public VagaService(
        IUnitOfWork unitOfWork,
        IValidator<VagaUpsertRequestDto> validator,
        ILogger<VagaService> logger)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    /// <summary>
    /// Cria ou edita uma vaga
    /// </summary>
    public async Task<Result<VagaResponseDto>> UpsertAsync(
        Guid empresaId,
        Guid usuarioId,
        VagaUpsertRequestDto request,
        CancellationToken cancellationToken = default)
    {
        // Validar
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Result.Fail<VagaResponseDto>(
                validationResult.Errors.Select(e => e.ErrorMessage));
        }

        if (request.Id.HasValue)
        {
            // Editar vaga existente
            var vagaExistente = await _unitOfWork.Vagas.GetByIdAsync(request.Id.Value, cancellationToken);
            if (vagaExistente == null)
                return Result.Fail<VagaResponseDto>("VAGA_NAO_ENCONTRADA");

            // Verificar se pertence à empresa
            if (vagaExistente.EmpresaId != empresaId)
                return Result.Fail<VagaResponseDto>("VAGA_NAO_PERTENCE_EMPRESA");

            // Atualizar dados
            request.Adapt(vagaExistente);
            _unitOfWork.Vagas.Update(vagaExistente);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Vaga atualizada: {Id} - {Titulo}", vagaExistente.Id, vagaExistente.Titulo);

            var responseDto = vagaExistente.Adapt<VagaResponseDto>();
            responseDto.EmpresaNome = (await _unitOfWork.Empresas.GetByIdAsync(empresaId, cancellationToken))?.Nome ?? "";
            return Result.Ok(responseDto);
        }
        else
        {
            // Criar nova vaga
            var vaga = Vaga.Criar(
                empresaId: empresaId,
                usuarioCriacaoId: usuarioId,
                titulo: request.Titulo,
                cargo: request.Cargo,
                descricao: request.Descricao,
                salario: request.Salario,
                dataInicio: request.DataCandidaturaInicio,
                dataFim: request.DataCandidaturaFim);

            // Preencher dados adicionais
            request.Adapt(vaga);

            await _unitOfWork.Vagas.AddAsync(vaga, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Adicionar competências (se houver)
            if (request.Competencias?.Any() == true)
            {
                foreach (var comp in request.Competencias)
                {
                    var competenciaVaga = new CompetenciaVaga(
                        vagaId: vaga.Id,
                        competenciaId: comp.CompetenciaId,
                        nivel: (CompetenciaNivelEnum)comp.Nivel,
                        peso: comp.Peso);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }
            }

            _logger.LogInformation("Vaga criada: {Id} - {Titulo} | Empresa: {EmpresaId}", vaga.Id, vaga.Titulo, empresaId);

            var responseDto = vaga.Adapt<VagaResponseDto>();
            responseDto.EmpresaNome = (await _unitOfWork.Empresas.GetByIdAsync(empresaId, cancellationToken))?.Nome ?? "";
            return Result.Ok(responseDto);
        }
    }

    /// <summary>
    /// Busca vagas ativas com filtros
    /// </summary>
    public async Task<Result<PaginacaoResponseDto<VagaResponseDto>>> BuscarVagasAsync(
        BuscarVagasRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var vagas = await _unitOfWork.Vagas.BuscarVagasAtivasAsync(
            termo: request.Termo,
            cidade: request.Cidade,
            estado: request.Estado,
            areaAtuacao: request.AreaAtuacao,
            regimeTrabalho: request.RegimeTrabalho,
            tipoContratacao: request.TipoContratacao,
            pagina: request.PageNumber,
            tamanhoPagina: request.PageSize,
            cancellationToken: cancellationToken);

        var vagasDto = vagas.Adapt<IEnumerable<VagaResponseDto>>();

        return Result.Ok(new PaginacaoResponseDto<VagaResponseDto>
        {
            Data = vagasDto,
            MetaData = new MetadadosPaginacaoDto
            {
                CurrentPage = request.PageNumber,
                PageSize = request.PageSize
            }
        });
    }

    /// <summary>
    /// Busca vaga por ID
    /// </summary>
    public async Task<Result<VagaResponseDto>> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var vaga = await _unitOfWork.Vagas.GetByIdAsync(id, cancellationToken);
        if (vaga == null)
            return Result.Fail<VagaResponseDto>("VAGA_NAO_ENCONTRADA");

        var empresa = await _unitOfWork.Empresas.GetByIdAsync(vaga.EmpresaId, cancellationToken);
        var dto = vaga.Adapt<VagaResponseDto>();
        dto.EmpresaNome = empresa?.Nome ?? "";
        dto.Status = vaga.Status.ToString();
        dto.TipoVaga = vaga.TipoVaga.ToString();

        return Result.Ok(dto);
    }

    /// <summary>
    /// Lista vagas de uma empresa
    /// </summary>
    public async Task<Result<IEnumerable<VagaResponseDto>>> GetByEmpresaAsync(
        Guid empresaId,
        CancellationToken cancellationToken = default)
    {
        var vagas = await _unitOfWork.Vagas.GetByEmpresaIdAsync(empresaId, cancellationToken);
        var empresa = await _unitOfWork.Empresas.GetByIdAsync(empresaId, cancellationToken);

        var dtos = vagas.Select(v =>
        {
            var dto = v.Adapt<VagaResponseDto>();
            dto.EmpresaNome = empresa?.Nome ?? "";
            dto.Status = v.Status.ToString();
            return dto;
        });

        return Result.Ok(dtos);
    }

    public async Task<Result<PaginacaoResponseDto<VagaResponseDto>>> GetByEmpresaPaginadoAsync(
        Guid empresaId,
        PaginacaoRequestDto paginacao,
        CancellationToken cancellationToken = default)
    {
        var total = await _unitOfWork.Vagas.CountByEmpresaIdAsync(empresaId, cancellationToken);
        var vagas = await _unitOfWork.Vagas.GetByEmpresaIdPaginadoAsync(
            empresaId, paginacao.PageNumber, paginacao.PageSize, cancellationToken);

        var empresa = await _unitOfWork.Empresas.GetByIdAsync(empresaId, cancellationToken);

        var dtos = vagas.Select(v =>
        {
            var dto = v.Adapt<VagaResponseDto>();
            dto.EmpresaNome = empresa?.Nome ?? "";
            dto.Status = v.Status.ToString();
            return dto;
        });

        var totalPages = (int)Math.Ceiling(total / (double)paginacao.PageSize);

        return Result.Ok(new PaginacaoResponseDto<VagaResponseDto>
        {
            Data = dtos,
            MetaData = new MetadadosPaginacaoDto
            {
                CurrentPage = paginacao.PageNumber,
                TotalCount = total,
                PageSize = paginacao.PageSize,
                TotalPages = totalPages,
                HasPrevious = paginacao.PageNumber > 1,
                HasNext = paginacao.PageNumber < totalPages
            }
        });
    }

    /// <summary>
    /// Encerra uma vaga
    /// </summary>
    public async Task<Result> EncerrarAsync(
        Guid vagaId,
        Guid usuarioId,
        CancellationToken cancellationToken = default)
    {
        var vaga = await _unitOfWork.Vagas.GetByIdAsync(vagaId, cancellationToken);
        if (vaga == null)
            return Result.Fail("VAGA_NAO_ENCONTRADA");

        vaga.Encerrar(usuarioId);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Vaga encerrada: {Id}", vagaId);
        return Result.Ok();
    }

    /// <summary>
    /// Reativa uma vaga
    /// </summary>
    public async Task<Result> ReativarAsync(
        Guid vagaId,
        CancellationToken cancellationToken = default)
    {
        var vaga = await _unitOfWork.Vagas.GetByIdAsync(vagaId, cancellationToken);
        if (vaga == null)
            return Result.Fail("VAGA_NAO_ENCONTRADA");

        vaga.Reativar();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Vaga reativada: {Id}", vagaId);
        return Result.Ok();
    }
}
